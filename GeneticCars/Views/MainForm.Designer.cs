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
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        simulationSKGLControl = new SkiaSharp.Views.Desktop.SKGLControl();
        mainSplitContainer = new SplitContainer();
        topPanel = new Panel();
        legendEliteLabel = new Label();
        legendMutationLabel = new Label();
        legendCrossLabel = new Label();
        legendNewLabel = new Label();
        mutationSizeComboBox = new ComboBox();
        parametersBindingSource = new BindingSource(components);
        percentValuesBindingSource1 = new BindingSource(components);
        mutationRateComboBox = new ComboBox();
        percentValuesBindingSource = new BindingSource(components);
        label5 = new Label();
        label4 = new Label();
        label3 = new Label();
        label2 = new Label();
        label1 = new Label();
        speedDisplayLabel = new Label();
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
        ((System.ComponentModel.ISupportInitialize)percentValuesBindingSource1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)percentValuesBindingSource).BeginInit();
        ((System.ComponentModel.ISupportInitialize)speedTrackBar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)zoomTrackBar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
        SuspendLayout();
        // 
        // simulationSKGLControl
        // 
        simulationSKGLControl.BackColor = Color.Black;
        simulationSKGLControl.Dock = DockStyle.Fill;
        simulationSKGLControl.Location = new Point(0, 101);
        simulationSKGLControl.Margin = new Padding(4, 3, 4, 3);
        simulationSKGLControl.Name = "simulationSKGLControl";
        simulationSKGLControl.Size = new Size(1211, 413);
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
        topPanel.Controls.Add(legendEliteLabel);
        topPanel.Controls.Add(legendMutationLabel);
        topPanel.Controls.Add(legendCrossLabel);
        topPanel.Controls.Add(legendNewLabel);
        topPanel.Controls.Add(mutationSizeComboBox);
        topPanel.Controls.Add(mutationRateComboBox);
        topPanel.Controls.Add(label5);
        topPanel.Controls.Add(label4);
        topPanel.Controls.Add(label3);
        topPanel.Controls.Add(label2);
        topPanel.Controls.Add(label1);
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
        topPanel.Size = new Size(1211, 101);
        topPanel.TabIndex = 1;
        // 
        // legendEliteLabel
        // 
        legendEliteLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        legendEliteLabel.AutoSize = true;
        legendEliteLabel.Location = new Point(1100, 30);
        legendEliteLabel.Name = "legendEliteLabel";
        legendEliteLabel.Size = new Size(92, 15);
        legendEliteLabel.TabIndex = 25;
        legendEliteLabel.Text = "▬ Elite, survivor";
        // 
        // legendMutationLabel
        // 
        legendMutationLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        legendMutationLabel.AutoSize = true;
        legendMutationLabel.Location = new Point(1100, 72);
        legendMutationLabel.Name = "legendMutationLabel";
        legendMutationLabel.Size = new Size(71, 15);
        legendMutationLabel.TabIndex = 24;
        legendMutationLabel.Text = "▬ Mutation";
        // 
        // legendCrossLabel
        // 
        legendCrossLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        legendCrossLabel.AutoSize = true;
        legendCrossLabel.Location = new Point(1100, 51);
        legendCrossLabel.Name = "legendCrossLabel";
        legendCrossLabel.Size = new Size(86, 15);
        legendCrossLabel.TabIndex = 23;
        legendCrossLabel.Text = "▬ Cross-breed";
        // 
        // legendNewLabel
        // 
        legendNewLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        legendNewLabel.AutoSize = true;
        legendNewLabel.Location = new Point(1100, 9);
        legendNewLabel.Name = "legendNewLabel";
        legendNewLabel.Size = new Size(99, 15);
        legendNewLabel.TabIndex = 22;
        legendNewLabel.Text = "▬ New (random)";
        // 
        // mutationSizeComboBox
        // 
        mutationSizeComboBox.DataBindings.Add(new Binding("SelectedValue", parametersBindingSource, "MutationSize", true, DataSourceUpdateMode.OnPropertyChanged));
        mutationSizeComboBox.DataSource = percentValuesBindingSource1;
        mutationSizeComboBox.DisplayMember = "Text";
        mutationSizeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        mutationSizeComboBox.FormattingEnabled = true;
        mutationSizeComboBox.Location = new Point(529, 70);
        mutationSizeComboBox.MaxDropDownItems = 20;
        mutationSizeComboBox.Name = "mutationSizeComboBox";
        mutationSizeComboBox.Size = new Size(62, 23);
        mutationSizeComboBox.TabIndex = 21;
        mutationSizeComboBox.ValueMember = "Value";
        // 
        // parametersBindingSource
        // 
        parametersBindingSource.DataSource = typeof(Parameters);
        // 
        // percentValuesBindingSource1
        // 
        percentValuesBindingSource1.DataMember = "PercentValues";
        percentValuesBindingSource1.DataSource = parametersBindingSource;
        // 
        // mutationRateComboBox
        // 
        mutationRateComboBox.DataBindings.Add(new Binding("SelectedValue", parametersBindingSource, "MutationRate", true, DataSourceUpdateMode.OnPropertyChanged));
        mutationRateComboBox.DataSource = percentValuesBindingSource;
        mutationRateComboBox.DisplayMember = "Text";
        mutationRateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        mutationRateComboBox.FormattingEnabled = true;
        mutationRateComboBox.Location = new Point(417, 70);
        mutationRateComboBox.MaxDropDownItems = 20;
        mutationRateComboBox.Name = "mutationRateComboBox";
        mutationRateComboBox.Size = new Size(62, 23);
        mutationRateComboBox.TabIndex = 20;
        mutationRateComboBox.ValueMember = "Value";
        // 
        // percentValuesBindingSource
        // 
        percentValuesBindingSource.DataMember = "PercentValues";
        percentValuesBindingSource.DataSource = parametersBindingSource;
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new Point(496, 73);
        label5.Margin = new Padding(0);
        label5.Name = "label5";
        label5.Size = new Size(30, 15);
        label5.TabIndex = 19;
        label5.Text = "Size:";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new Point(381, 73);
        label4.Margin = new Padding(0);
        label4.Name = "label4";
        label4.Size = new Size(33, 15);
        label4.TabIndex = 18;
        label4.Text = "Rate:";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(316, 73);
        label3.Name = "label3";
        label3.Size = new Size(56, 15);
        label3.TabIndex = 17;
        label3.Text = "Mutation";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(19, 9);
        label2.Name = "label2";
        label2.Size = new Size(42, 15);
        label2.TabIndex = 16;
        label2.Text = "Speed:";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(648, 9);
        label1.Name = "label1";
        label1.Size = new Size(48, 15);
        label1.TabIndex = 15;
        label1.Text = "Display:";
        // 
        // speedDisplayLabel
        // 
        speedDisplayLabel.AutoSize = true;
        speedDisplayLabel.BorderStyle = BorderStyle.Fixed3D;
        speedDisplayLabel.DataBindings.Add(new Binding("Text", parametersBindingSource, "SpeedDisplay", true, DataSourceUpdateMode.OnPropertyChanged));
        speedDisplayLabel.Location = new Point(151, 28);
        speedDisplayLabel.Margin = new Padding(3, 0, 15, 0);
        speedDisplayLabel.Name = "speedDisplayLabel";
        speedDisplayLabel.Size = new Size(40, 17);
        speedDisplayLabel.TabIndex = 14;
        speedDisplayLabel.Text = "label1";
        // 
        // speedTrackBar
        // 
        speedTrackBar.AutoSize = false;
        speedTrackBar.DataBindings.Add(new Binding("Value", parametersBindingSource, "Iterations", true, DataSourceUpdateMode.OnPropertyChanged));
        speedTrackBar.LargeChange = 4;
        speedTrackBar.Location = new Point(12, 27);
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
        zoomNameLabel.Location = new Point(330, 41);
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
        zoomTrackBar.Location = new Point(378, 41);
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
        displayHealthBarCheckBox.Location = new Point(648, 52);
        displayHealthBarCheckBox.Name = "displayHealthBarCheckBox";
        displayHealthBarCheckBox.Padding = new Padding(4, 0, 0, 0);
        displayHealthBarCheckBox.Size = new Size(65, 19);
        displayHealthBarCheckBox.TabIndex = 10;
        displayHealthBarCheckBox.Text = "Health";
        displayHealthBarCheckBox.UseVisualStyleBackColor = true;
        // 
        // displayNamesCheckBox
        // 
        displayNamesCheckBox.AutoSize = true;
        displayNamesCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "DisplayNames", true, DataSourceUpdateMode.OnPropertyChanged));
        displayNamesCheckBox.Location = new Point(648, 27);
        displayNamesCheckBox.Name = "displayNamesCheckBox";
        displayNamesCheckBox.Padding = new Padding(4, 0, 0, 0);
        displayNamesCheckBox.Size = new Size(62, 19);
        displayNamesCheckBox.TabIndex = 9;
        displayNamesCheckBox.Text = "Name";
        displayNamesCheckBox.UseVisualStyleBackColor = true;
        // 
        // populationSIzeNumberLabel
        // 
        populationSIzeNumberLabel.AutoSize = true;
        populationSIzeNumberLabel.BorderStyle = BorderStyle.Fixed3D;
        populationSIzeNumberLabel.DataBindings.Add(new Binding("Text", parametersBindingSource, "PopulationSize", true, DataSourceUpdateMode.OnPropertyChanged));
        populationSIzeNumberLabel.Location = new Point(589, 12);
        populationSIzeNumberLabel.Margin = new Padding(3, 0, 15, 0);
        populationSIzeNumberLabel.Name = "populationSIzeNumberLabel";
        populationSIzeNumberLabel.Size = new Size(40, 17);
        populationSIzeNumberLabel.TabIndex = 8;
        populationSIzeNumberLabel.Text = "label1";
        // 
        // populationSizeNameLabel
        // 
        populationSizeNameLabel.AutoSize = true;
        populationSizeNameLabel.Location = new Point(304, 12);
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
        trackBar1.Location = new Point(378, 12);
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
        deathCheckBox.Location = new Point(247, 50);
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
        kryptoniteCheckBox.Location = new Point(209, 50);
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
        radioactivityCheckBox.Location = new Point(247, 12);
        radioactivityCheckBox.Name = "radioactivityCheckBox";
        radioactivityCheckBox.Size = new Size(32, 32);
        radioactivityCheckBox.TabIndex = 3;
        toolTip1.SetToolTip(radioactivityCheckBox, "Radioactivity");
        radioactivityCheckBox.UseVisualStyleBackColor = false;
        // 
        // displayFpsCheckBox
        // 
        displayFpsCheckBox.AutoSize = true;
        displayFpsCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "DisplayFps", true, DataSourceUpdateMode.OnPropertyChanged));
        displayFpsCheckBox.Location = new Point(648, 77);
        displayFpsCheckBox.Name = "displayFpsCheckBox";
        displayFpsCheckBox.Padding = new Padding(4, 0, 0, 0);
        displayFpsCheckBox.Size = new Size(49, 19);
        displayFpsCheckBox.TabIndex = 2;
        displayFpsCheckBox.Text = "FPS";
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
        changeFloorCheckBox.Location = new Point(209, 12);
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
        pauseResumeButton.Location = new Point(19, 56);
        pauseResumeButton.Name = "pauseResumeButton";
        pauseResumeButton.Size = new Size(129, 26);
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
        Icon = (Icon)resources.GetObject("$this.Icon");
        MinimumSize = new Size(850, 355);
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
        ((System.ComponentModel.ISupportInitialize)percentValuesBindingSource1).EndInit();
        ((System.ComponentModel.ISupportInitialize)percentValuesBindingSource).EndInit();
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
    private Label label1;
    private Label label2;
    private ComboBox mutationRateComboBox;
    private Label label5;
    private Label label4;
    private Label label3;
    private ComboBox mutationSizeComboBox;
    private BindingSource percentValuesBindingSource;
    private BindingSource percentValuesBindingSource1;
    private Label legendEliteLabel;
    private Label legendMutationLabel;
    private Label legendCrossLabel;
    private Label legendNewLabel;
}
