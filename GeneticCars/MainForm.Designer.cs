﻿namespace GeneticCars;

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
        skGLControl = new SkiaSharp.Views.Desktop.SKGLControl();
        SuspendLayout();
        // 
        // skGLControl
        // 
        skGLControl.BackColor = Color.Black;
        skGLControl.Location = new Point(13, 12);
        skGLControl.Margin = new Padding(4, 3, 4, 3);
        skGLControl.Name = "skGLControl";
        skGLControl.Size = new Size(1185, 426);
        skGLControl.TabIndex = 0;
        skGLControl.VSync = true;
        skGLControl.PaintSurface += SkGLControl_PaintSurface;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1211, 450);
        Controls.Add(skGLControl);
        Name = "MainForm";
        Text = "Evolving Cars";
        Load += MainForm_Load;
        ResumeLayout(false);
    }

    #endregion

    private SkiaSharp.Views.Desktop.SKGLControl skGLControl;
}