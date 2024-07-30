namespace GeneticCars;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        simulationSKGLControl = new SkiaSharp.Views.Desktop.SKGLControl();
        mainSplitContainer = new SplitContainer();
        topPanel = new Panel();
        speedDisplayLabel = new Label();
        parametersBindingSource = new BindingSource(components);
        speedTrackBar = new TrackBar();
        zoomNameLabel = new Label();
        zoomTrackBar = new TrackBar();
        displayHealthBarCheckBox = new CheckBox();
        displayNamesCheckBox = new CheckBox();
        populationSIzeNumberLabel = new Label();
        populationSizeNameLabel = new Label();
        trackBar1 = new TrackBar();
        deathCheckBox = new CheckBox();
        kryptoniteCheckBox = new CheckBox();
        radioactivityCheckBox = new CheckBox();
        displayFpsCheckBox = new CheckBox();
        changeFloorCheckBox = new CheckBox();
        pauseResumeButton = new Button();
        familyTreeVPanel = new Views.Controls.ScrollableSKGLControl();
        toolTip1 = new ToolTip(components);
        ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
        mainSplitContainer.Panel1.SuspendLayout();
        mainSplitContainer.Panel2.SuspendLayout();
        mainSplitContainer.SuspendLayout();
        topPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)parametersBindingSource).BeginInit();
        ((System.ComponentModel.ISupportInitialize)speedTrackBar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)zoomTrackBar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
        SuspendLayout();
        // 
        // simulationSKGLControl
        // 
        simulationSKGLControl.BackColor = Color.Black;
        simulationSKGLControl.Dock = DockStyle.Fill;
        simulationSKGLControl.Location = new Point(0, 62);
        simulationSKGLControl.Margin = new Padding(4, 3, 4, 3);
        simulationSKGLControl.Name = "simulationSKGLControl";
        simulationSKGLControl.Size = new Size(1211, 452);
        simulationSKGLControl.TabIndex = 0;
        simulationSKGLControl.VSync = true;
        simulationSKGLControl.PaintSurface += SimulationSKGLControl_PaintSurface;
        // 
        // mainSplitContainer
        // 
        mainSplitContainer.Dock = DockStyle.Fill;
        mainSplitContainer.Location = new Point(0, 0);
        mainSplitContainer.Name = "mainSplitContainer";
        mainSplitContainer.Orientation = Orientation.Horizontal;
        // 
        // mainSplitContainer.Panel1
        // 
        mainSplitContainer.Panel1.Controls.Add(simulationSKGLControl);
        mainSplitContainer.Panel1.Controls.Add(topPanel);
        // 
        // mainSplitContainer.Panel2
        // 
        mainSplitContainer.Panel2.Controls.Add(familyTreeVPanel);
        mainSplitContainer.Size = new Size(1211, 822);
        mainSplitContainer.SplitterDistance = 514;
        mainSplitContainer.TabIndex = 1;
        // 
        // topPanel
        // 
        topPanel.Controls.Add(speedDisplayLabel);
        topPanel.Controls.Add(speedTrackBar);
        topPanel.Controls.Add(zoomNameLabel);
        topPanel.Controls.Add(zoomTrackBar);
        topPanel.Controls.Add(displayHealthBarCheckBox);
        topPanel.Controls.Add(displayNamesCheckBox);
        topPanel.Controls.Add(populationSIzeNumberLabel);
        topPanel.Controls.Add(populationSizeNameLabel);
        topPanel.Controls.Add(trackBar1);
        topPanel.Controls.Add(deathCheckBox);
        topPanel.Controls.Add(kryptoniteCheckBox);
        topPanel.Controls.Add(radioactivityCheckBox);
        topPanel.Controls.Add(displayFpsCheckBox);
        topPanel.Controls.Add(changeFloorCheckBox);
        topPanel.Controls.Add(pauseResumeButton);
        topPanel.Dock = DockStyle.Top;
        topPanel.Location = new Point(0, 0);
        topPanel.Name = "topPanel";
        topPanel.Size = new Size(1211, 62);
        topPanel.TabIndex = 1;
        // 
        // speedDisplayLabel
        // 
        speedDisplayLabel.AutoSize = true;
        speedDisplayLabel.BorderStyle = BorderStyle.Fixed3D;
        speedDisplayLabel.DataBindings.Add(new Binding("Text", parametersBindingSource, "SpeedDisplay", true, DataSourceUpdateMode.OnPropertyChanged));
        speedDisplayLabel.Location = new Point(153, 8);
        speedDisplayLabel.Margin = new Padding(3, 0, 15, 0);
        speedDisplayLabel.Name = "speedDisplayLabel";
        speedDisplayLabel.Size = new Size(40, 17);
        speedDisplayLabel.TabIndex = 14;
        speedDisplayLabel.Text = "label1";
        // 
        // parametersBindingSource
        // 
        parametersBindingSource.DataSource = typeof(Parameters);
        // 
        // speedTrackBar
        // 
        speedTrackBar.AutoSize = false;
        speedTrackBar.DataBindings.Add(new Binding("Value", parametersBindingSource, "Iterations", true, DataSourceUpdateMode.OnPropertyChanged));
        speedTrackBar.LargeChange = 4;
        speedTrackBar.Location = new Point(12, 8);
        speedTrackBar.Margin = new Padding(3, 3, 15, 3);
        speedTrackBar.Maximum = 16;
        speedTrackBar.Minimum = 1;
        speedTrackBar.Name = "speedTrackBar";
        speedTrackBar.Size = new Size(144, 23);
        speedTrackBar.TabIndex = 13;
        speedTrackBar.TickStyle = TickStyle.None;
        toolTip1.SetToolTip(speedTrackBar, "Speed");
        speedTrackBar.Value = 4;
        // 
        // zoomNameLabel
        // 
        zoomNameLabel.AutoSize = true;
        zoomNameLabel.Location = new Point(405, 37);
        zoomNameLabel.Name = "zoomNameLabel";
        zoomNameLabel.Size = new Size(42, 15);
        zoomNameLabel.TabIndex = 12;
        zoomNameLabel.Text = "Zoom:";
        // 
        // zoomTrackBar
        // 
        zoomTrackBar.AutoSize = false;
        zoomTrackBar.DataBindings.Add(new Binding("Value", parametersBindingSource, "Zoom", true, DataSourceUpdateMode.OnPropertyChanged));
        zoomTrackBar.LargeChange = 15;
        zoomTrackBar.Location = new Point(453, 37);
        zoomTrackBar.Maximum = 60;
        zoomTrackBar.Minimum = 15;
        zoomTrackBar.Name = "zoomTrackBar";
        zoomTrackBar.Size = new Size(214, 23);
        zoomTrackBar.SmallChange = 5;
        zoomTrackBar.TabIndex = 11;
        zoomTrackBar.TickFrequency = 4;
        zoomTrackBar.TickStyle = TickStyle.None;
        zoomTrackBar.Value = 15;
        // 
        // displayHealthBarCheckBox
        // 
        displayHealthBarCheckBox.AutoSize = true;
        displayHealthBarCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "DisplayHealthBar", true, DataSourceUpdateMode.OnPropertyChanged));
        displayHealthBarCheckBox.Location = new Point(722, 37);
        displayHealthBarCheckBox.Name = "displayHealthBarCheckBox";
        displayHealthBarCheckBox.Size = new Size(122, 19);
        displayHealthBarCheckBox.TabIndex = 10;
        displayHealthBarCheckBox.Text = "Display health-bar";
        displayHealthBarCheckBox.UseVisualStyleBackColor = true;
        // 
        // displayNamesCheckBox
        // 
        displayNamesCheckBox.AutoSize = true;
        displayNamesCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "DisplayNames", true, DataSourceUpdateMode.OnPropertyChanged));
        displayNamesCheckBox.Location = new Point(722, 12);
        displayNamesCheckBox.Name = "displayNamesCheckBox";
        displayNamesCheckBox.Size = new Size(102, 19);
        displayNamesCheckBox.TabIndex = 9;
        displayNamesCheckBox.Text = "Display names";
        displayNamesCheckBox.UseVisualStyleBackColor = true;
        // 
        // populationSIzeNumberLabel
        // 
        populationSIzeNumberLabel.AutoSize = true;
        populationSIzeNumberLabel.BorderStyle = BorderStyle.Fixed3D;
        populationSIzeNumberLabel.DataBindings.Add(new Binding("Text", parametersBindingSource, "PopulationSize", true, DataSourceUpdateMode.OnPropertyChanged));
        populationSIzeNumberLabel.Location = new Point(664, 8);
        populationSIzeNumberLabel.Margin = new Padding(3, 0, 15, 0);
        populationSIzeNumberLabel.Name = "populationSIzeNumberLabel";
        populationSIzeNumberLabel.Size = new Size(40, 17);
        populationSIzeNumberLabel.TabIndex = 8;
        populationSIzeNumberLabel.Text = "label1";
        // 
        // populationSizeNameLabel
        // 
        populationSizeNameLabel.AutoSize = true;
        populationSizeNameLabel.Location = new Point(379, 8);
        populationSizeNameLabel.Name = "populationSizeNameLabel";
        populationSizeNameLabel.Size = new Size(68, 15);
        populationSizeNameLabel.TabIndex = 7;
        populationSizeNameLabel.Text = "Population:";
        // 
        // trackBar1
        // 
        trackBar1.AutoSize = false;
        trackBar1.DataBindings.Add(new Binding("Value", parametersBindingSource, "PopulationSize", true, DataSourceUpdateMode.OnPropertyChanged));
        trackBar1.LargeChange = 20;
        trackBar1.Location = new Point(453, 8);
        trackBar1.Maximum = 200;
        trackBar1.Minimum = 8;
        trackBar1.Name = "trackBar1";
        trackBar1.Size = new Size(214, 23);
        trackBar1.SmallChange = 4;
        trackBar1.TabIndex = 6;
        trackBar1.TickFrequency = 4;
        trackBar1.TickStyle = TickStyle.None;
        trackBar1.Value = 8;
        // 
        // deathCheckBox
        // 
        deathCheckBox.Appearance = Appearance.Button;
        deathCheckBox.BackColor = SystemColors.Control;
        deathCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "Death.Value", true, DataSourceUpdateMode.OnPropertyChanged));
        deathCheckBox.DataBindings.Add(new Binding("Enabled", parametersBindingSource, "Death.Enabled", true, DataSourceUpdateMode.OnPropertyChanged));
        deathCheckBox.DataBindings.Add(new Binding("Image", parametersBindingSource, "Death.Image", true, DataSourceUpdateMode.OnPropertyChanged));
        deathCheckBox.FlatAppearance.BorderSize = 0;
        deathCheckBox.FlatStyle = FlatStyle.Flat;
        deathCheckBox.Image = Properties.Resources.DeathUp32;
        deathCheckBox.Location = new Point(329, 12);
        deathCheckBox.Margin = new Padding(3, 3, 15, 3);
        deathCheckBox.Name = "deathCheckBox";
        deathCheckBox.Size = new Size(32, 32);
        deathCheckBox.TabIndex = 5;
        toolTip1.SetToolTip(deathCheckBox, "Death");
        deathCheckBox.UseVisualStyleBackColor = false;
        // 
        // kryptoniteCheckBox
        // 
        kryptoniteCheckBox.Appearance = Appearance.Button;
        kryptoniteCheckBox.BackColor = SystemColors.Control;
        kryptoniteCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "Kryptonite.Value", true, DataSourceUpdateMode.OnPropertyChanged));
        kryptoniteCheckBox.DataBindings.Add(new Binding("Enabled", parametersBindingSource, "Kryptonite.Enabled", true, DataSourceUpdateMode.OnPropertyChanged));
        kryptoniteCheckBox.DataBindings.Add(new Binding("Image", parametersBindingSource, "Kryptonite.Image", true, DataSourceUpdateMode.OnPropertyChanged));
        kryptoniteCheckBox.FlatAppearance.BorderSize = 0;
        kryptoniteCheckBox.FlatStyle = FlatStyle.Flat;
        kryptoniteCheckBox.Image = Properties.Resources.Kryptonite32Up;
        kryptoniteCheckBox.Location = new Point(291, 12);
        kryptoniteCheckBox.Name = "kryptoniteCheckBox";
        kryptoniteCheckBox.Size = new Size(32, 32);
        kryptoniteCheckBox.TabIndex = 4;
        toolTip1.SetToolTip(kryptoniteCheckBox, "Kryptonite");
        kryptoniteCheckBox.UseVisualStyleBackColor = false;
        // 
        // radioactivityCheckBox
        // 
        radioactivityCheckBox.Appearance = Appearance.Button;
        radioactivityCheckBox.BackColor = SystemColors.Control;
        radioactivityCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "Radioactivity.Value", true, DataSourceUpdateMode.OnPropertyChanged));
        radioactivityCheckBox.DataBindings.Add(new Binding("Enabled", parametersBindingSource, "Radioactivity.Enabled", true, DataSourceUpdateMode.OnPropertyChanged));
        radioactivityCheckBox.DataBindings.Add(new Binding("Image", parametersBindingSource, "Radioactivity.Image", true, DataSourceUpdateMode.OnPropertyChanged));
        radioactivityCheckBox.FlatAppearance.BorderSize = 0;
        radioactivityCheckBox.FlatStyle = FlatStyle.Flat;
        radioactivityCheckBox.Image = Properties.Resources.RadioactiveUp32;
        radioactivityCheckBox.Location = new Point(253, 12);
        radioactivityCheckBox.Name = "radioactivityCheckBox";
        radioactivityCheckBox.Size = new Size(32, 32);
        radioactivityCheckBox.TabIndex = 3;
        toolTip1.SetToolTip(radioactivityCheckBox, "Radioactivity");
        radioactivityCheckBox.UseVisualStyleBackColor = false;
        // 
        // displayFpsCheckBox
        // 
        displayFpsCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        displayFpsCheckBox.AutoSize = true;
        displayFpsCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "DisplayFps", true, DataSourceUpdateMode.OnPropertyChanged));
        displayFpsCheckBox.Location = new Point(1113, 12);
        displayFpsCheckBox.Name = "displayFpsCheckBox";
        displayFpsCheckBox.Size = new Size(86, 19);
        displayFpsCheckBox.TabIndex = 2;
        displayFpsCheckBox.Text = "Display FPS";
        displayFpsCheckBox.UseVisualStyleBackColor = true;
        // 
        // changeFloorCheckBox
        // 
        changeFloorCheckBox.Appearance = Appearance.Button;
        changeFloorCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "RegenerateFloor.Value", true, DataSourceUpdateMode.OnPropertyChanged));
        changeFloorCheckBox.DataBindings.Add(new Binding("Image", parametersBindingSource, "RegenerateFloor.Image", true, DataSourceUpdateMode.OnPropertyChanged));
        changeFloorCheckBox.FlatAppearance.BorderSize = 0;
        changeFloorCheckBox.FlatStyle = FlatStyle.Flat;
        changeFloorCheckBox.Image = Properties.Resources.ChangeFloorUp32;
        changeFloorCheckBox.Location = new Point(203, 13);
        changeFloorCheckBox.Margin = new Padding(3, 3, 15, 3);
        changeFloorCheckBox.Name = "changeFloorCheckBox";
        changeFloorCheckBox.Size = new Size(32, 32);
        changeFloorCheckBox.TabIndex = 1;
        toolTip1.SetToolTip(changeFloorCheckBox, "Regenerate Floor");
        changeFloorCheckBox.UseVisualStyleBackColor = true;
        // 
        // pauseResumeButton
        // 
        pauseResumeButton.DataBindings.Add(new Binding("Text", parametersBindingSource, "PlayButtonText", true, DataSourceUpdateMode.OnPropertyChanged));
        pauseResumeButton.DataBindings.Add(new Binding("Command", parametersBindingSource, "PlayCommand", true));
        pauseResumeButton.Location = new Point(19, 34);
        pauseResumeButton.Name = "pauseResumeButton";
        pauseResumeButton.Size = new Size(129, 23);
        pauseResumeButton.TabIndex = 0;
        pauseResumeButton.Text = "pauseResumeButton";
        pauseResumeButton.UseVisualStyleBackColor = true;
        // 
        // familyTreeVPanel
        // 
        familyTreeVPanel.Dock = DockStyle.Fill;
        familyTreeVPanel.LargeChange = new Size(100, 100);
        familyTreeVPanel.Location = new Point(0, 0);
        familyTreeVPanel.Name = "familyTreeVPanel";
        familyTreeVPanel.ScrollOffset = new Point(0, 0);
        familyTreeVPanel.Size = new Size(1211, 304);
        familyTreeVPanel.SmallChange = new Size(10, 10);
        familyTreeVPanel.TabIndex = 0;
        familyTreeVPanel.VirtualAreaSize = new Size(0, 0);
        familyTreeVPanel.PaintSurface += FamilyTreeVPanel_PaintSurface;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1211, 822);
        Controls.Add(mainSplitContainer);
        Name = "MainForm";
        Text = "Evolving Cars";
        Load += MainForm_Load;
        mainSplitContainer.Panel1.ResumeLayout(false);
        mainSplitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
        mainSplitContainer.ResumeLayout(false);
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)parametersBindingSource).EndInit();
        ((System.ComponentModel.ISupportInitialize)speedTrackBar).EndInit();
        ((System.ComponentModel.ISupportInitialize)zoomTrackBar).EndInit();
        ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private SkiaSharp.Views.Desktop.SKGLControl simulationSKGLControl;
    private SplitContainer mainSplitContainer;
    private Panel topPanel;
    private Button pauseResumeButton;
    private BindingSource parametersBindingSource;
    private CheckBox changeFloorCheckBox;
    private Views.Controls.ScrollableSKGLControl familyTreeVPanel;
    private CheckBox displayFpsCheckBox;
    private CheckBox radioactivityCheckBox;
    private CheckBox kryptoniteCheckBox;
    private CheckBox deathCheckBox;
    private ToolTip toolTip1;
    private TrackBar trackBar1;
    private Label populationSizeNameLabel;
    private Label populationSIzeNumberLabel;
    private CheckBox displayNamesCheckBox;
    private CheckBox displayHealthBarCheckBox;
    private Label zoomNameLabel;
    private TrackBar zoomTrackBar;
    private TrackBar speedTrackBar;
    private Label speedDisplayLabel;
}
