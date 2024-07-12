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
        changingFloorCheckBox = new CheckBox();
        parametersBindingSource = new BindingSource(components);
        button1 = new Button();
        flowLayoutPanel1 = new FlowLayoutPanel();
        familyTreeSKGLControl = new SkiaSharp.Views.Desktop.SKGLControl();
        ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
        mainSplitContainer.Panel1.SuspendLayout();
        mainSplitContainer.Panel2.SuspendLayout();
        mainSplitContainer.SuspendLayout();
        topPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)parametersBindingSource).BeginInit();
        flowLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // simulationSKGLControl
        // 
        simulationSKGLControl.BackColor = Color.Black;
        simulationSKGLControl.Dock = DockStyle.Fill;
        simulationSKGLControl.Location = new Point(0, 57);
        simulationSKGLControl.Margin = new Padding(4, 3, 4, 3);
        simulationSKGLControl.Name = "simulationSKGLControl";
        simulationSKGLControl.Size = new Size(1211, 457);
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
        mainSplitContainer.Panel2.Controls.Add(flowLayoutPanel1);
        mainSplitContainer.Size = new Size(1211, 822);
        mainSplitContainer.SplitterDistance = 514;
        mainSplitContainer.TabIndex = 1;
        // 
        // topPanel
        // 
        topPanel.Controls.Add(changingFloorCheckBox);
        topPanel.Controls.Add(button1);
        topPanel.Dock = DockStyle.Top;
        topPanel.Location = new Point(0, 0);
        topPanel.Name = "topPanel";
        topPanel.Size = new Size(1211, 57);
        topPanel.TabIndex = 1;
        // 
        // changingFloorCheckBox
        // 
        changingFloorCheckBox.AutoSize = true;
        changingFloorCheckBox.DataBindings.Add(new Binding("Checked", parametersBindingSource, "ChangingFloor", true, DataSourceUpdateMode.OnPropertyChanged));
        changingFloorCheckBox.Location = new Point(114, 15);
        changingFloorCheckBox.Name = "changingFloorCheckBox";
        changingFloorCheckBox.Size = new Size(166, 19);
        changingFloorCheckBox.TabIndex = 1;
        changingFloorCheckBox.Text = "Change Floor Every Round";
        changingFloorCheckBox.UseVisualStyleBackColor = true;
        // 
        // parametersBindingSource
        // 
        parametersBindingSource.DataSource = typeof(Parameters);
        // 
        // button1
        // 
        button1.DataBindings.Add(new Binding("Text", parametersBindingSource, "PlayButtonText", true, DataSourceUpdateMode.OnPropertyChanged));
        button1.DataBindings.Add(new Binding("Command", parametersBindingSource, "PlayCommand", true));
        button1.Location = new Point(12, 12);
        button1.Name = "button1";
        button1.Size = new Size(75, 23);
        button1.TabIndex = 0;
        button1.Text = "pauseResumeButton";
        button1.UseVisualStyleBackColor = true;
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.AutoScroll = true;
        flowLayoutPanel1.AutoSize = true;
        flowLayoutPanel1.BackColor = Color.White;
        flowLayoutPanel1.Controls.Add(familyTreeSKGLControl);
        flowLayoutPanel1.Dock = DockStyle.Fill;
        flowLayoutPanel1.Location = new Point(0, 0);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new Size(1211, 304);
        flowLayoutPanel1.TabIndex = 2;
        flowLayoutPanel1.Scroll += FlowLayoutPanel1_Scroll;
        flowLayoutPanel1.Resize += FlowLayoutPanel_Resize;
        // 
        // familyTreeSKGLControl
        // 
        familyTreeSKGLControl.BackColor = Color.Black;
        familyTreeSKGLControl.Location = new Point(4, 3);
        familyTreeSKGLControl.Margin = new Padding(4, 3, 4, 3);
        familyTreeSKGLControl.Name = "familyTreeSKGLControl";
        familyTreeSKGLControl.Size = new Size(309, 97);
        familyTreeSKGLControl.TabIndex = 1;
        familyTreeSKGLControl.VSync = true;
        familyTreeSKGLControl.PaintSurface += FamilyTreeSKGLControl_PaintSurface;
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
        mainSplitContainer.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
        mainSplitContainer.ResumeLayout(false);
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)parametersBindingSource).EndInit();
        flowLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private SkiaSharp.Views.Desktop.SKGLControl simulationSKGLControl;
    private SplitContainer mainSplitContainer;
    private Panel topPanel;
    private SkiaSharp.Views.Desktop.SKGLControl familyTreeSKGLControl;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button button1;
    private BindingSource parametersBindingSource;
    private CheckBox changingFloorCheckBox;
}
