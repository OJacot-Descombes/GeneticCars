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
        familyTreeVPanel = new Views.Controls.VPanel();
        ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
        mainSplitContainer.Panel1.SuspendLayout();
        mainSplitContainer.Panel2.SuspendLayout();
        mainSplitContainer.SuspendLayout();
        topPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)parametersBindingSource).BeginInit();
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
        mainSplitContainer.Panel2.Controls.Add(familyTreeVPanel);
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
        ResumeLayout(false);
    }

    #endregion

    private SkiaSharp.Views.Desktop.SKGLControl simulationSKGLControl;
    private SplitContainer mainSplitContainer;
    private Panel topPanel;
    private Button button1;
    private BindingSource parametersBindingSource;
    private CheckBox changingFloorCheckBox;
    private Views.Controls.VPanel familyTreeVPanel;
}
