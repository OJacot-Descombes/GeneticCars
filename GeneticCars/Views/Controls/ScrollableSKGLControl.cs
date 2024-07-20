using System.ComponentModel;
using SkiaSharp.Views.Desktop;
using WinForms = System.Windows.Forms;

namespace GeneticCars.Views.Controls;

[DefaultEvent(nameof(PaintSurface))]
public partial class ScrollableSKGLControl : UserControl
{
    private const int SBWIDTH = 16; // Width of vertical and height of horizontal scrollbar. Hight and width of buttons.
    private const int MINSLIDERSIZE = 10; // Minimum height of vertical and minimum width of horizontal slider.
    private const int MINBTNSIZE = 5; // Minimum size of buttons at which they are visible.

    private int _oversizeX = 0, _oversizeY = 0;  // Oversize of slider, when minimum slider size under-run

    public ScrollableSKGLControl()
    {
        InitializeComponent();
        timer.Tick += Timer_Tick;
    }

    private EventHandler? _timerAction;

    public event EventHandler<SKScrollEventArgs>? ClientAreaScroll;

    public delegate void PaintSurfaceHandler(SKPaintGLSurfaceEventArgs e, SKRect viewBox);

    public event PaintSurfaceHandler? PaintSurface;

    #region Public Properties

    private Size virtualAreaSize;
    public Size VirtualAreaSize
    {
        get { return virtualAreaSize; }
        set {
            virtualAreaSize = value;
            HandleResize();
        }
    }

    private Point scrollOffset;
    public Point ScrollOffset
    {
        get { return scrollOffset; }
        set {
            Point oldScrollOffset = scrollOffset;

            scrollOffset = value;
            SetVerticalSlider();
            SetHorizontalSlider();
            PerformUpdate(oldScrollOffset, false);
        }
    }

    private Size smallChange = new(10, 10);
    public Size SmallChange
    {
        get { return smallChange; }
        set { smallChange = value; }
    }

    private Size largeChange = new(100, 100);
    public Size LargeChange
    {
        get { return largeChange; }
        set { largeChange = value; }
    }

    public SKGLControl ClientArea => skGLControl;

    #endregion

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _timerAction?.Invoke(sender, e);
    }

    private void PerformUpdate(Point oldScrollOffset, bool callOnClientAreaScroll)
    {

        int dx = scrollOffset.X - oldScrollOffset.X;
        int dy = scrollOffset.Y - oldScrollOffset.Y;
        if (dx != 0 || dy != 0) {
            skGLControl.Invalidate();
            skGLControl.Update();
            if (callOnClientAreaScroll) {
                ClientAreaScroll?.Invoke(this, new SKScrollEventArgs(dx, dy));
            }
        }
    }

    private void HandleResize()
    {
        bool vVisible, hVisible;
        int vSBWidth, hSBWidth;
        Size size;

        // Determine visibility and resulting width of scrollbars
        vVisible = ClientSize.Height < virtualAreaSize.Height; // Assume horizontal scrollbar not visible.
        hVisible = ClientSize.Width - (vVisible ? SBWIDTH : 0) < virtualAreaSize.Width;
        if (!vVisible && hVisible) { // Recalculate visibility of vertical scrollbar.
            vVisible = ClientSize.Height - SBWIDTH < virtualAreaSize.Height;
        }
        hSBWidth = hVisible ? SBWIDTH : 0;
        vSBWidth = vVisible ? SBWIDTH : 0;

        size = new Size(ClientSize.Width - vSBWidth, ClientSize.Height - hSBWidth);
        if (skGLControl.Size != size) {
            skGLControl.Size = size;
        }

        if (vVisible) {
            int h = ClientSize.Height - hSBWidth;  // Height of the vertical scroll bar.

            // Set background of scrollbar (tracker)
            lblTrackVertical.Height = h;
            lblTrackVertical.Location = new Point(ClientSize.Width - SBWIDTH, 0);
            lblTrackVertical.Visible = true;

            // Determine visibility of vertical slider
            lblVerticalSlider.Visible = h - 2 * SBWIDTH > MINSLIDERSIZE;

            // Set up/down buttons
            if (h >= 2 * MINBTNSIZE) { // Buttons are visible
                if (h >= 2 * SBWIDTH) { // Buttons have normal size
                    btnScrollUp.Height = SBWIDTH;
                    btnScrollDn.Height = SBWIDTH;
                } else { // Buttons have reduced size
                    btnScrollUp.Height = h / 2;
                    btnScrollDn.Height = h - btnScrollUp.Height;
                }
                btnScrollDn.Location = new Point(ClientSize.Width - SBWIDTH, h - btnScrollDn.Height);
                btnScrollUp.Visible = true;
                btnScrollDn.Visible = true;
            } else {
                btnScrollUp.Visible = false;
                btnScrollDn.Visible = false;
            }
        } else {
            btnScrollUp.Visible = false;
            btnScrollDn.Visible = false;
            lblVerticalSlider.Visible = false;
            lblTrackVertical.Visible = false;
        }

        if (hVisible) {
            int w = ClientSize.Width - vSBWidth;  // Width of the horizontal scroll bar.

            // Set background of scrollbar (tracker)
            lblTrackHorizontal.Width = w;
            lblTrackHorizontal.Location = new Point(0, ClientSize.Height - SBWIDTH);
            lblTrackHorizontal.Visible = true;

            // Determine visibility of horizontal slider
            lblHorizontalSlider.Visible = w - 2 * SBWIDTH > MINSLIDERSIZE;

            // Set up/down buttons
            if (w >= 2 * MINBTNSIZE) { // Buttons are visible
                if (w >= 2 * SBWIDTH) { // Buttons have normal size
                    btnScrollLt.Width = SBWIDTH;
                    btnScrollRt.Width = SBWIDTH;
                } else { // Buttons have reduced size
                    btnScrollLt.Width = w / 2;
                    btnScrollRt.Width = w - btnScrollLt.Width;
                }
                btnScrollRt.Location = new Point(w - btnScrollRt.Width, ClientSize.Height - SBWIDTH);
                btnScrollLt.Visible = true;
                btnScrollRt.Visible = true;
            } else {
                btnScrollLt.Visible = false;
                btnScrollRt.Visible = false;
            }
        } else {
            btnScrollLt.Visible = false;
            btnScrollRt.Visible = false;
            lblHorizontalSlider.Visible = false;
            lblTrackHorizontal.Visible = false;
        }

        Point oldScrollOffset = scrollOffset;
        SetVerticalSlider();
        SetHorizontalSlider();
        PerformUpdate(oldScrollOffset, true);
    }

    private void SetSlider(bool vertical, Label slider, int clientSize, int virtualSize, out int oversize, ref int scrollOffset)
    {
        int trackSize = clientSize - 2 * SBWIDTH;  // Subtract both buttons
        virtualSize = Math.Max(1, virtualSize); // In order to avoid division by zero
        int sliderSize = Convert.ToInt32((double)clientSize / virtualSize * trackSize);

        oversize = 0;
        if (sliderSize < MINSLIDERSIZE) {
            oversize = MINSLIDERSIZE - sliderSize;
            sliderSize = MINSLIDERSIZE;
        }
        if (clientSize + scrollOffset > virtualSize) {
            scrollOffset = Math.Max(0, virtualSize - clientSize);
        }
        if (slider.Visible) {
            int sliderPos = Convert.ToInt32((double)scrollOffset / virtualSize * (trackSize - oversize));
            Point newLocation;
            if (vertical) {
                newLocation = new Point(ClientSize.Width - SBWIDTH, sliderPos + SBWIDTH);
                if (newLocation != slider.Location || slider.Height != sliderSize) {
                    slider.Height = sliderSize;
                    slider.Location = newLocation;
                    lblTrackVertical.Update();
                    slider.Update();
                }
            } else {
                newLocation = new Point(sliderPos + SBWIDTH, ClientSize.Height - SBWIDTH);
                if (newLocation != slider.Location || slider.Width != sliderSize) {
                    slider.Width = sliderSize;
                    slider.Location = newLocation;
                    lblTrackHorizontal.Update();
                    slider.Update();
                }
            }
        }
    }

    private void SetVerticalSlider()
    {
        int y = scrollOffset.Y;
        SetSlider(true, lblVerticalSlider, skGLControl.Height, virtualAreaSize.Height, out _oversizeY, ref y);
        scrollOffset.Y = y;
    }

    private void SetHorizontalSlider()
    {
        int x = scrollOffset.X;
        SetSlider(false, lblHorizontalSlider, skGLControl.Width, virtualAreaSize.Width, out _oversizeX, ref x);
        scrollOffset.X = x;
    }

    #region Small and large scrolling steps

    private const int AUTOREPEAT_DELAY = 400, AUTOREPEAT_INTERVALL = 50;
    private Size change;

    private void ScrollUp(object? sender, EventArgs e)
    {
        Point oldScrollOffset = scrollOffset;

        scrollOffset.Y = Math.Max(0, scrollOffset.Y - change.Height);
        SetVerticalSlider();
        PerformUpdate(oldScrollOffset, true);
        if (sender is WinForms.Timer timer) {
            timer.Interval = AUTOREPEAT_INTERVALL;
        }
    }

    private void ScrollDn(object? sender, EventArgs e)
    {
        Point oldScrollOffset = scrollOffset;

        scrollOffset.Y = Math.Min(virtualAreaSize.Height - skGLControl.Height, scrollOffset.Y + change.Height);
        SetVerticalSlider();
        PerformUpdate(oldScrollOffset, true);
        if (sender is WinForms.Timer timer) {
            timer.Interval = AUTOREPEAT_INTERVALL;
        }
    }

    private void ScrollLt(object? sender, EventArgs e)
    {
        Point oldScrollOffset = scrollOffset;

        scrollOffset.X = Math.Max(0, scrollOffset.X - change.Width);
        SetHorizontalSlider();
        PerformUpdate(oldScrollOffset, true);
        if (sender is WinForms.Timer timer) {
            timer.Interval = AUTOREPEAT_INTERVALL;
        }
    }

    private void ScrollRt(object? sender, EventArgs e)
    {
        Point oldScrollOffset = scrollOffset;

        scrollOffset.X = Math.Min(virtualAreaSize.Width - skGLControl.Width, scrollOffset.X + change.Width);
        SetHorizontalSlider();
        PerformUpdate(oldScrollOffset, true);
        if (sender is WinForms.Timer timer) {
            timer.Interval = AUTOREPEAT_INTERVALL;
        }
    }

    private void StartAutorepeatScroll(EventHandler ScrollAction, Size change)
    {
        this.change = change;
        ScrollAction(this, EventArgs.Empty);
        if (timer.Enabled) {
            timer.Stop();
        }
        _timerAction = ScrollAction;
        timer.Interval = AUTOREPEAT_DELAY;
        timer.Start();
    }

    private void StopAutorepeatScroll()
    {
        if (timer.Enabled) {
            timer.Stop();
        }
        skGLControl.Focus();
        OnScrollEnd();
    }

    private void BtnScrollUp_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) {
            StartAutorepeatScroll(ScrollUp, smallChange);
        }
    }

    private void BtnScrollUp_MouseUp(object sender, MouseEventArgs e)
    {
        StopAutorepeatScroll();
    }

    private void BtnScrollDn_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) {
            StartAutorepeatScroll(ScrollDn, smallChange);
        }
    }

    private void BtnScrollDn_MouseUp(object sender, MouseEventArgs e)
    {
        StopAutorepeatScroll();
    }

    private void BtnScrollLt_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) {
            StartAutorepeatScroll(ScrollLt, smallChange);
        }
    }

    private void BtnScrollLt_MouseUp(object sender, MouseEventArgs e)
    {
        StopAutorepeatScroll();
    }

    private void BtnScrollRt_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) {
            StartAutorepeatScroll(ScrollRt, smallChange);
        }
    }

    private void BtnScrollRt_MouseUp(object sender, MouseEventArgs e)
    {
        StopAutorepeatScroll();
    }

    private void LblTrackVertical_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && lblVerticalSlider.Visible) {
            if (PointToClient(MousePosition).Y < lblVerticalSlider.Top) { // User clicked above slider
                StartAutorepeatScroll(ScrollUp, largeChange);
            } else { // User clicked below slider
                StartAutorepeatScroll(ScrollDn, largeChange);
            }
        }
    }

    private void LblTrackVertical_MouseUp(object sender, MouseEventArgs e)
    {
        StopAutorepeatScroll();
    }

    private void LblTrackHorizontal_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && lblHorizontalSlider.Visible) {
            if (PointToClient(MousePosition).X < lblHorizontalSlider.Left) { // User clicked to the left of the slider
                StartAutorepeatScroll(ScrollLt, largeChange);
            } else { // User clicked to the right of the slider
                StartAutorepeatScroll(ScrollRt, largeChange);
            }
        }
    }

    private void LblTrackHorizontal_MouseUp(object sender, MouseEventArgs e)
    {
        StopAutorepeatScroll();
    }

    #endregion

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        HandleResize();
    }

    protected override void OnLoad(EventArgs e)
    {
        // Caution: This event is fired each time the control is docked around in the dock panel suite.
        base.OnLoad(e);
        HandleResize();
    }

    #region Moving sliders with the mouse

    private bool sliding = false;
    private bool scrollOccurred = false;  // Tells if scrolling occurred while sliding
    private int mouseOffset;

    private int GetScrollOffset(int mousePos, int sliderSize, int clientSize, int virtualSize, int oversize, ref int sliderPos)
    {
        sliderPos = sliderPos + mousePos - mouseOffset;
        if (sliderPos <= SBWIDTH) { // Slider must not be placed before left/upper button
            sliderPos = SBWIDTH;
            return 0;
        }
        if (sliderPos + sliderSize >= clientSize - SBWIDTH) { // Slider must not be placed after right/lower button
            sliderPos = clientSize - SBWIDTH - sliderSize;
            return virtualSize - clientSize;
        }
        return Convert.ToInt32((double)(sliderPos - SBWIDTH) / (clientSize - 2 * SBWIDTH - oversize) * virtualSize);
    }

    private void LblVerticalSlider_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) {
            sliding = true;
            scrollOccurred = false;
            mouseOffset = e.Y;
        }
    }

    private void LblVerticalSlider_MouseUp(object sender, MouseEventArgs e)
    {
        sliding = false;
        if (scrollOccurred) {
            OnScrollEnd();
        }
    }

    public event EventHandler? ScrollEnd;

    private void OnScrollEnd()
    {
        ScrollEnd?.Invoke(this, EventArgs.Empty);
    }

    private void LblVerticalSlider_MouseMove(object sender, MouseEventArgs e)
    {
        if (sliding) {
            Point oldScrollOffset = scrollOffset;
            int pos = lblVerticalSlider.Top;

            scrollOccurred = true;
            scrollOffset.Y = GetScrollOffset(e.Y, lblVerticalSlider.Height, skGLControl.Height, virtualAreaSize.Height, _oversizeY, ref pos);
            PerformUpdate(oldScrollOffset, true);
            lblVerticalSlider.Location = new Point(ClientSize.Width - SBWIDTH, pos);
            lblTrackVertical.Update();
        }
    }

    private void LblHorizontalSlider_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) {
            sliding = true;
            scrollOccurred = false;
            mouseOffset = e.X;
        }
    }

    private void LblHorizontalSlider_MouseUp(object sender, MouseEventArgs e)
    {
        sliding = false;
        if (scrollOccurred) {
            OnScrollEnd();
        }
    }

    private void LblHorizontalSlider_MouseMove(object sender, MouseEventArgs e)
    {
        if (sliding) {
            Point oldScrollOffset = scrollOffset;
            int pos = lblHorizontalSlider.Left;

            scrollOccurred = true;
            scrollOffset.X = GetScrollOffset(e.X, lblHorizontalSlider.Width, skGLControl.Width, virtualAreaSize.Width, _oversizeX, ref pos);
            PerformUpdate(oldScrollOffset, true);
            lblHorizontalSlider.Location = new Point(pos, ClientSize.Height - SBWIDTH);
            lblTrackHorizontal.Update();
        }
    }

    #endregion

    private void SkGLControl_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        if (PaintSurface != null) {
            var viewBox = new SKRect(ScrollOffset.X, ScrollOffset.Y,
                ScrollOffset.X + skGLControl.Width, ScrollOffset.Y + skGLControl.Height);
            PaintSurface(e, viewBox);
        }
    }
}