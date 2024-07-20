namespace GeneticCars.Views.Controls
{
    partial class ScrollableSKGLControl {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnScrollUp = new NoFocusButton();
            lblTrackVertical = new Label();
            btnScrollDn = new NoFocusButton();
            lblVerticalSlider = new Label();
            btnScrollLt = new NoFocusButton();
            btnScrollRt = new NoFocusButton();
            lblTrackHorizontal = new Label();
            lblHorizontalSlider = new Label();
            skGLControl = new SkiaSharp.Views.Desktop.SKGLControl();
            timer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // btnScrollUp
            // 
            btnScrollUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnScrollUp.BackColor = SystemColors.Control;
            btnScrollUp.CausesValidation = false;
            btnScrollUp.FlatStyle = FlatStyle.Popup;
            btnScrollUp.Image = Properties.Resources.UpArrow;
            btnScrollUp.Location = new Point(284, 0);
            btnScrollUp.Margin = new Padding(0);
            btnScrollUp.Name = "btnScrollUp";
            btnScrollUp.Size = new Size(16, 16);
            btnScrollUp.TabIndex = 2;
            btnScrollUp.TabStop = false;
            btnScrollUp.UseVisualStyleBackColor = false;
            btnScrollUp.MouseDown += BtnScrollUp_MouseDown;
            btnScrollUp.MouseUp += BtnScrollUp_MouseUp;
            // 
            // lblTrackVertical
            // 
            lblTrackVertical.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lblTrackVertical.BackColor = Color.WhiteSmoke;
            lblTrackVertical.Location = new Point(284, 0);
            lblTrackVertical.Margin = new Padding(0);
            lblTrackVertical.Name = "lblTrackVertical";
            lblTrackVertical.Size = new Size(16, 284);
            lblTrackVertical.TabIndex = 1;
            lblTrackVertical.MouseDown += LblTrackVertical_MouseDown;
            lblTrackVertical.MouseUp += LblTrackVertical_MouseUp;
            // 
            // btnScrollDn
            // 
            btnScrollDn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnScrollDn.BackColor = SystemColors.Control;
            btnScrollDn.CausesValidation = false;
            btnScrollDn.FlatStyle = FlatStyle.Popup;
            btnScrollDn.Image = Properties.Resources.DnArrow;
            btnScrollDn.Location = new Point(284, 268);
            btnScrollDn.Margin = new Padding(0);
            btnScrollDn.Name = "btnScrollDn";
            btnScrollDn.Size = new Size(16, 16);
            btnScrollDn.TabIndex = 4;
            btnScrollDn.TabStop = false;
            btnScrollDn.UseVisualStyleBackColor = false;
            btnScrollDn.MouseDown += BtnScrollDn_MouseDown;
            btnScrollDn.MouseUp += BtnScrollDn_MouseUp;
            // 
            // lblVerticalSlider
            // 
            lblVerticalSlider.Anchor = AnchorStyles.Right;
            lblVerticalSlider.BackColor = SystemColors.Control;
            lblVerticalSlider.BorderStyle = BorderStyle.FixedSingle;
            lblVerticalSlider.CausesValidation = false;
            lblVerticalSlider.Location = new Point(284, 112);
            lblVerticalSlider.Margin = new Padding(0);
            lblVerticalSlider.Name = "lblVerticalSlider";
            lblVerticalSlider.Size = new Size(16, 26);
            lblVerticalSlider.TabIndex = 3;
            lblVerticalSlider.MouseDown += LblVerticalSlider_MouseDown;
            lblVerticalSlider.MouseMove += LblVerticalSlider_MouseMove;
            lblVerticalSlider.MouseUp += LblVerticalSlider_MouseUp;
            // 
            // btnScrollLt
            // 
            btnScrollLt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnScrollLt.BackColor = SystemColors.Control;
            btnScrollLt.CausesValidation = false;
            btnScrollLt.FlatStyle = FlatStyle.Popup;
            btnScrollLt.Image = Properties.Resources.LtArrow;
            btnScrollLt.Location = new Point(0, 284);
            btnScrollLt.Margin = new Padding(0);
            btnScrollLt.Name = "btnScrollLt";
            btnScrollLt.Size = new Size(16, 16);
            btnScrollLt.TabIndex = 6;
            btnScrollLt.TabStop = false;
            btnScrollLt.UseVisualStyleBackColor = false;
            btnScrollLt.MouseDown += BtnScrollLt_MouseDown;
            btnScrollLt.MouseUp += BtnScrollLt_MouseUp;
            // 
            // btnScrollRt
            // 
            btnScrollRt.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnScrollRt.BackColor = SystemColors.Control;
            btnScrollRt.CausesValidation = false;
            btnScrollRt.FlatStyle = FlatStyle.Popup;
            btnScrollRt.Image = Properties.Resources.RtArrow;
            btnScrollRt.Location = new Point(268, 284);
            btnScrollRt.Margin = new Padding(0);
            btnScrollRt.Name = "btnScrollRt";
            btnScrollRt.Size = new Size(16, 16);
            btnScrollRt.TabIndex = 8;
            btnScrollRt.TabStop = false;
            btnScrollRt.UseVisualStyleBackColor = false;
            btnScrollRt.MouseDown += BtnScrollRt_MouseDown;
            btnScrollRt.MouseUp += BtnScrollRt_MouseUp;
            // 
            // lblTrackHorizontal
            // 
            lblTrackHorizontal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblTrackHorizontal.BackColor = Color.WhiteSmoke;
            lblTrackHorizontal.Location = new Point(0, 284);
            lblTrackHorizontal.Margin = new Padding(0);
            lblTrackHorizontal.Name = "lblTrackHorizontal";
            lblTrackHorizontal.Size = new Size(284, 16);
            lblTrackHorizontal.TabIndex = 5;
            lblTrackHorizontal.MouseDown += LblTrackHorizontal_MouseDown;
            lblTrackHorizontal.MouseUp += LblTrackHorizontal_MouseUp;
            // 
            // lblHorizontalSlider
            // 
            lblHorizontalSlider.Anchor = AnchorStyles.Bottom;
            lblHorizontalSlider.BackColor = SystemColors.Control;
            lblHorizontalSlider.BorderStyle = BorderStyle.FixedSingle;
            lblHorizontalSlider.CausesValidation = false;
            lblHorizontalSlider.Location = new Point(140, 284);
            lblHorizontalSlider.Margin = new Padding(0);
            lblHorizontalSlider.Name = "lblHorizontalSlider";
            lblHorizontalSlider.Size = new Size(60, 16);
            lblHorizontalSlider.TabIndex = 7;
            lblHorizontalSlider.MouseDown += LblHorizontalSlider_MouseDown;
            lblHorizontalSlider.MouseMove += LblHorizontalSlider_MouseMove;
            lblHorizontalSlider.MouseUp += LblHorizontalSlider_MouseUp;
            // 
            // skGLControl
            // 
            skGLControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            skGLControl.BackColor = Color.White;
            skGLControl.Location = new Point(0, 0);
            skGLControl.Margin = new Padding(0);
            skGLControl.Name = "skGLControl";
            skGLControl.Size = new Size(284, 284);
            skGLControl.TabIndex = 0;
            skGLControl.VSync = true;
            skGLControl.PaintSurface += SkGLControl_PaintSurface;
            // 
            // VPanel
            // 
            Controls.Add(lblHorizontalSlider);
            Controls.Add(lblVerticalSlider);
            Controls.Add(btnScrollRt);
            Controls.Add(btnScrollLt);
            Controls.Add(btnScrollDn);
            Controls.Add(btnScrollUp);
            Controls.Add(skGLControl);
            Controls.Add(lblTrackHorizontal);
            Controls.Add(lblTrackVertical);
            Name = "VPanel";
            Size = new Size(300, 300);
            ResumeLayout(false);
        }

        #endregion

        private GeneticCars.Views.Controls.NoFocusButton btnScrollUp;
        private System.Windows.Forms.Label lblTrackVertical;
        private GeneticCars.Views.Controls.NoFocusButton btnScrollDn;
        private System.Windows.Forms.Label lblVerticalSlider;
        private GeneticCars.Views.Controls.NoFocusButton btnScrollLt;
        private GeneticCars.Views.Controls.NoFocusButton btnScrollRt;
        private System.Windows.Forms.Label lblTrackHorizontal;
        private System.Windows.Forms.Label lblHorizontalSlider;
        public SkiaSharp.Views.Desktop.SKGLControl skGLControl;
        private System.Windows.Forms.Timer timer;

    }
}
