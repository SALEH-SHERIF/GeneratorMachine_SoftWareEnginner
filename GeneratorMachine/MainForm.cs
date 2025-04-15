using GeneratorMachine.Services;
using GeneratorMachine.Services.Helpers;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace GeneratorMachine
{
    public partial class MainForm : Form
    {
        private readonly IGeneratorService _generatorService;
        private readonly BindingList<IQueueItem> _queueItems ;
        private readonly ConcurrentQueue<IComponentJob> _processingQueue;
        private BackgroundWorker _worker;
        private int _totalComponents;      // Total items in the current batch (fixed once added)
        private int _completedComponents;  // How many items have been completed
        private readonly object _counterLock = new object();

		// UI Controls
		#region UI Controls
		private ComboBox cmbType;
        private ComboBox cmbSubType;
        private TextBox txtQuantity;
        private Button btnAdd;
        private ListBox lstQueue;
        private ProgressBar progressBar;
        private Button btnContinue;
        private Label lblStatus;

        #endregion

        public MainForm(IGeneratorService creatorService , BindingList<IQueueItem> queueItems , ConcurrentQueue<IComponentJob> processingQueue)
        {
            _generatorService = creatorService ?? throw new ArgumentNullException(nameof(creatorService)); // Ensure service is not null
            _queueItems = queueItems;
            _processingQueue = processingQueue;
			InitializeComponent();
            SetupUI();
            InitializeWorker();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Creator Driver";
            // Allow resizing now.
            this.Size = new Size(950, 600);
            this.FormBorderStyle = FormBorderStyle.Sizable; // Allow resizing now.
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Title Label (Centered initially, but remains at top)
            Label lblTitle = new Label
            {
                Text = "Creator Driver",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.DarkBlue
            };
            // Center horizontally based on initial client size.
            lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, 20);
            this.Controls.Add(lblTitle);

            // Progress Bar Section
            Label lblProgress = new Label
            {
                Text = "Progress Bar",
                Location = new Point(20, 70),
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkGreen
            };
            this.Controls.Add(lblProgress);

            progressBar = new ProgressBar
            {
                Location = new Point(20, 100),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Maximum = 100,
                Style = ProgressBarStyle.Continuous,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(progressBar);

            lblStatus = new Label
            {
                Text = "No components added yet.",
                Location = new Point(20, 140),
                Size = new Size(this.ClientSize.Width - 40, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.Gray,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(lblStatus);

            // Components Queue Section
            Label lblQueue = new Label
            {
                Text = "COMPONENTS QUEUE",
                Location = new Point(20, 170),
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkGreen
            };
            this.Controls.Add(lblQueue);

            lstQueue = new ListBox
            {
                Location = new Point(20, 200),
                Size = new Size(300, this.ClientSize.Height - 220),
                BackColor = Color.LightGray,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            this.Controls.Add(lstQueue);

            // Component Selection Section on the right side.
            // Components Dropdown
            Label lblComponents = new Label
            {
                Text = "Components",
                Location = new Point(340, 200),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblComponents);

            cmbType = new ComboBox
            {
                Location = new Point(340, 230),
                Size = new Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            this.Controls.Add(cmbType);

            // Component Types Dropdown
            Label lblComponentTypes = new Label
            {
                Text = "Component Types",
                Location = new Point(340, 270),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblComponentTypes);

            cmbSubType = new ComboBox
            {
                Location = new Point(340, 300),
                Size = new Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            this.Controls.Add(cmbSubType);

            // Number of Components TextBox
            Label lblNumberOfComponents = new Label
            {
                Text = "Number of Components",
                Location = new Point(340, 340),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblNumberOfComponents);

            txtQuantity = new TextBox
            {
                Location = new Point(340, 370),
                Size = new Size(250, 30),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            this.Controls.Add(txtQuantity);

            // Add Button
            btnAdd = new Button
            {
                Text = "Add",
                Location = new Point(620, 365),
                Size = new Size(150, 40),
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            this.Controls.Add(btnAdd);

            // Continue Button (initially hidden)
            btnContinue = new Button
            {
                Text = "Continue",
                Location = new Point(620, 415),
                Size = new Size(150, 40),
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Visible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            this.Controls.Add(btnContinue);

            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Initialize type dropdown.
            cmbType.Items.AddRange(_generatorService.GetAllComponentTypes().ToArray());
            cmbType.SelectedIndexChanged += (s, e) => UpdateSubTypes();

            // Event handlers.
            btnAdd.Click += BtnAdd_Click;
            btnContinue.Click += BtnContinue_Click;

            // Queue list binding.
            lstQueue.DataSource = _queueItems;
            lstQueue.DisplayMember = "DisplayText";

            UpdateSubTypes();
        }

        private void InitializeWorker()
        {
            // Create a new background worker instance each time.
            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = false
            };
            _worker.DoWork += ProcessQueue;
            _worker.ProgressChanged += UpdateProgress;
            _worker.RunWorkerAsync();
        }

        private void UpdateSubTypes()
        {
            cmbSubType.Items.Clear();
            cmbSubType.Text = string.Empty;
            var type = cmbType.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(type))
            {
                cmbSubType.Items.AddRange(_generatorService.GetSubTypesForType(type).ToArray());
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Validate type selection.
            if (cmbType.SelectedItem == null)
            {
                MessageBox.Show("Please select a component.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbType.Focus();
                return;
            }

            // Validate subtype selection.
            if (cmbSubType.SelectedItem == null)
            {
                MessageBox.Show("Please select a component type.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSubType.Focus();
                return;
            }

            // Validate quantity.
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Please enter the number of components.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 1 || quantity > 1000000)
            {
                MessageBox.Show("Number of components must be a valid (integer) at least 1 and at most 1000000 .", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return;
            }

            // Create a new queue item.
            var component = new QueueItem(
                cmbType.SelectedItem.ToString(),
                cmbSubType.SelectedItem.ToString(),
                quantity
            );

            _queueItems.Add(component);

            lock (_counterLock)
            {
                // Increase global counter by the component's total quantity.
                _totalComponents += component.Quantity;
            }

            for (int i = 0; i < component.Quantity; i++)
            {
                _processingQueue.Enqueue(new ComponentJob
                {
                    Type = component.Type,
                    SubType = component.SubType,
                    ParentItem = component
                });
            }

            // Clear inputs.
            cmbType.SelectedIndex = -1;
            cmbSubType.Items.Clear();
            txtQuantity.Clear();
        }

        private void ProcessQueue(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (_processingQueue.TryDequeue(out var job))
                {
                    try
                    {
                        int progress;

                        // Show "Creating ..." before the actual creation.
                        this.Invoke((MethodInvoker)delegate {
                            lblStatus.Text = $"CREATING {job.SubType} {job.Type} (Component Number {_completedComponents + 1} in queue)";
                            lblStatus.ForeColor = Color.DarkBlue;
                        });

                        // Call the service method to create the component.
                        _generatorService.CreateComponent(job.Type, job.SubType);

                        // On success, increment the completed counter.
                        lock (_counterLock)
                        {
                            _completedComponents++;
                            progress = (int)((_completedComponents / (float)_totalComponents) * 100);
                        }

                        // Update status and progress bar.
                        this.Invoke((MethodInvoker)delegate {
                            lblStatus.Text = $"Successfully created {job.SubType} {job.Type}";
                            progressBar.Value = Math.Clamp(progress, 0, 100);
                        });

                        // Pause to allow the success message to show.
                        Thread.Sleep(2000);

                        // Update the queue item's completed count in the UI
                        // and remove the item from the list when fully processed.
                        this.Invoke((MethodInvoker)delegate {
                            job.ParentItem.Completed++;
                            int idx = _queueItems.IndexOf(job.ParentItem);
                            if (idx >= 0)
                            {
                                if (job.ParentItem.Completed >= job.ParentItem.Quantity)
                                {
                                    _queueItems.RemoveAt(idx);
                                }
                                else
                                {
                                    _queueItems.ResetItem(idx);
                                }
                            }
                        });

                        // Report progress.
                        _worker.ReportProgress(_completedComponents);
                    }
                    catch (Exception ex)
                    {
                        // Requeue the failed job.
                        _processingQueue.Enqueue(job);
                        // Display error and enable the Continue button.
                        this.Invoke((MethodInvoker)delegate {
                            btnContinue.Visible = true;
                            lblStatus.Text = $"Error: {ex.Message}. Click 'Continue' to retry.";
                            lblStatus.ForeColor = Color.DarkRed;
                        });
                        // Exit the processing loop to halt further processing.
                        return;
                    }
                    Thread.Sleep(500); // Simulate work delay.
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            lock (_counterLock)
            {
                int progress = _totalComponents > 0 ? (int)((_completedComponents / (float)_totalComponents) * 100) : 0;
                progressBar.Value = Math.Clamp(progress, 0, 100);

                // When the entire batch is complete, clear the UI.
                if (_completedComponents >= _totalComponents && _totalComponents > 0)
                {
                    lblStatus.Text = "Completed all components";
                    lblStatus.ForeColor = Color.DarkGreen;
                    _queueItems.Clear();
                    _totalComponents = 0;
                    _completedComponents = 0;
                    progressBar.Value = 0;
                }
            }
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            // Hide the continue button.
            btnContinue.Visible = false;

            // Resume processing by creating a new background worker.
            InitializeWorker();
        }

     
    }
}
