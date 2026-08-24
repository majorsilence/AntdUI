// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using Majorsilence.Forms;

namespace AntdUI
{
    public class BaseForm : Form
    {
        public BaseForm()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        public void SetCursor(bool val)
        {
            if (InvokeRequired)
            {
                Invoke(() => SetCursor(val));
                return;
            }
            Cursor = val ? Cursors.Hand : DefaultCursor;
        }

        #region 主题

        bool dark = false;
        /// <summary>
        /// 深色模式
        /// </summary>
        [Description("深色模式"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool Dark
        {
            get => dark;
            set
            {
                if (dark == value) return;
                dark = value;
                mode = dark ? TAMode.Dark : TAMode.Light;
                if (IsHandleCreated)
                {
                    if (Win32.WindowTheme(this, value)) SetThemeOK(value);
                }
            }
        }

        TAMode mode = TAMode.Auto;
        /// <summary>
        /// 色彩模式
        /// </summary>
        [Description("色彩模式"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TAMode.Auto)]
        public TAMode Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                if (mode == TAMode.Dark || (mode == TAMode.Auto || Config.Mode == TMode.Dark)) Dark = true;
                else Dark = false;
            }
        }

        /// <summary>
        /// 是否禁用主题
        /// </summary>
        [Description("是否禁用主题"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool DisableTheme { get; set; }

        internal ThemeConfig? themeConfig;
        public ThemeConfig Theme()
        {
            themeConfig = new ThemeConfig(this);
            if (Config.ThemeConfig != null) themeConfig.SetConfig(Config.ThemeConfig);
            return themeConfig;
        }

        public void ThemeClear() => themeConfig = null;

        internal void SetTheme()
        {
            if (DisableTheme) return;
            if (mode == TAMode.Auto)
            {
                if (themeConfig == null)
                {
                    if (Config.ThemeConfig != null)
                    {
                        themeConfig = new ThemeConfig(this);
                        themeConfig.SetConfig(Config.ThemeConfig);
                        SetTheme(themeConfig);
                    }
                }
                else SetTheme(themeConfig);
            }
            bool dark = mode == TAMode.Dark || (mode == TAMode.Auto && Config.Mode == TMode.Dark);
            if (Win32.WindowTheme(this, dark, true)) SetThemeOK(dark);
        }
        protected virtual void SetThemeOK(bool dark) { }
        internal void SetTheme(ThemeConfig themeConfig)
        {
            if (themeConfig.pageheader == null && (themeConfig.headerLight.HasValue || themeConfig.headerDark.HasValue)) themeConfig.pageheader = GetPageHeader(Controls);
            dark = Config.IsDark;
            themeConfig.Change(dark);
            EventHub.Add(this);
        }
        PageHeader? GetPageHeader(Control.ControlCollection controls)
        {
            foreach (Control it in controls)
            {
                if (it is PageHeader header) return header;
                else if (it.Controls.Count > 0)
                {
                    var tmp = GetPageHeader(it.Controls);
                    if (tmp == null) continue;
                    return tmp;
                }
            }
            return null;
        }

        #endregion

        #region 程序

        FormBorderStyle formBorderStyle = FormBorderStyle.Sizable;
        [Description("指示窗体的边框和标题栏的外观和行为"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(FormBorderStyle.Sizable)]
        public new FormBorderStyle FormBorderStyle
        {
            get => formBorderStyle;
            set
            {
                if (formBorderStyle == value) return;
                base.FormBorderStyle = formBorderStyle = value;
            }
        }

        public virtual void RefreshDWM() { }

        /// <summary>
        /// 最小化
        /// </summary>
        public virtual void Min() => WindowState = FormWindowState.Minimized;

        public virtual bool IsMax => WindowState == FormWindowState.Maximized;

        /// <summary>
        /// 最大化/还原
        /// </summary>
        public virtual bool MaxRestore()
        {
            if (IsFull)
            {
                base.FormBorderStyle = formBorderStyle;
                IsFull = false;
            }
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                RefreshDWM();
                return false;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                RefreshDWM();
                return true;
            }
        }

        /// <summary>
        /// 最大化
        /// </summary>
        public virtual void Max()
        {
            if (IsFull)
            {
                base.FormBorderStyle = formBorderStyle;
                IsFull = false;
            }
            WindowState = FormWindowState.Maximized;
            RefreshDWM();
        }

        /// <summary>
        /// 全屏/还原
        /// </summary>
        public virtual bool FullRestore()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                NoFull();
                return false;
            }
            else
            {
                Full();
                return true;
            }
        }

        public bool IsFull = false;
        /// <summary>
        /// 全屏
        /// </summary>
        public virtual void Full()
        {
            if (IsFull) return;
            IsFull = true;
            base.FormBorderStyle = FormBorderStyle.None;
            if (WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal;
            WindowState = FormWindowState.Maximized;
            RefreshDWM();
        }

        public virtual void NoFull()
        {
            if (IsFull)
            {
                IsFull = false;
                base.FormBorderStyle = formBorderStyle;
                WindowState = FormWindowState.Normal;
                RefreshDWM();
            }
            else if (IsMax) MaxRestore();
        }

        #endregion

        #region DPI

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public virtual bool AutoHandDpi { get; set; } = true;


        float? dpi;
        public float Dpi
        {
            get
            {
                if (Config._dpi_custom.HasValue) return Config._dpi_custom.Value;
                if (dpi.HasValue) return dpi.Value;
                var _dpi = Helper.GetScreenDpi(this);
                dpi = _dpi;
                return _dpi;
            }
        }

        public void AutoDpi(Control control) => AutoDpi(Dpi, control);

        public void AutoDpi(float dpi, Control control) => Helper.DpiAuto(dpi, control);

        public void AutoDpi(Form form) => AutoDpi(Dpi, form);

        public void AutoDpi(float dpi, Form form) => Helper.DpiAuto(dpi, form);

        protected override void WndProc(ref Majorsilence.Forms.Message m)
        {
            if (m.Msg == 0x02E0)
            {
                var old = Dpi;
                // 低字节是水平DPI，高字节是垂直DPI
                int dpiX = (int)(m.WParam.ToInt64() & 0xFFFF), dpiY = (int)(m.WParam.ToInt64() >> 16);
                var _dpi = Helper.GetDpi(dpiX, dpiY);
                dpi = _dpi;
                if (AutoHandDpi) BeginInvoke(() => Helper.DpiChangeAuto(_dpi, old, this));
            }
            base.WndProc(ref m);
        }

        bool isload = false;
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (isload || DesignMode) return;
            OnCreated‌();
            SetTheme();
            if (AutoHandDpi) AutoDpi(Dpi, this);
            isload = true;
        }

        public virtual void OnCreated‌()
        {
        }

        #endregion

        #region 交互

        #region 拖动窗口

        /// <summary>
        /// 拖动窗口（鼠标按下）
        /// </summary>
        public virtual void DraggableMouseDown()
        {
            if (IsFull) return;
            // ReleaseCapture + WM_NCLBUTTONDOWN(HTCAPTION) is the Win32 idiom for "let the user drag
            // the window from here". Majorsilence.Forms exposes the same thing directly, and off
            // Windows the P/Invokes are absent -- so use it rather than throwing on a title-bar drag.
            if (!OperatingSystem.IsWindows())
            {
                BeginMoveDrag();
                return;
            }
            Win32.User32.ReleaseCapture();
            Win32.User32.SendMessage(Handle, 0x0112, 61456 | 2, IntPtr.Zero);
        }

        #endregion

        #region 调整窗口大小

        /// <summary>
        /// 调整窗口大小（鼠标移动）
        /// </summary>
        /// <returns>可以调整</returns>
        public virtual bool ResizableMouseMove()
        {
            if (WindowState == FormWindowState.Normal)
            {
                var retval = HitTest(PointToClient(MousePosition));
                if (retval != Win32.User32.HitTestValues.HTNOWHERE)
                {
                    var mode = retval;
                    if (mode != Win32.User32.HitTestValues.HTCLIENT)
                    {
                        SetCursorHit(mode);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 调整窗口大小（鼠标移动）
        /// </summary>
        /// <param name="point">客户端坐标</param>
        /// <returns>可以调整</returns>
        public virtual bool ResizableMouseMove(Point point)
        {
            if (WindowState == FormWindowState.Normal)
            {
                var retval = HitTest(point);
                if (retval != Win32.User32.HitTestValues.HTNOWHERE)
                {
                    var mode = retval;
                    if (mode != Win32.User32.HitTestValues.HTCLIENT)
                    {
                        SetCursorHit(mode);
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool is_resizable;
        /// <summary>
        /// 整窗口大小（鼠标按下）
        /// </summary>
        /// <returns>可以调整</returns>
        public virtual bool ResizableMouseDown()
        {
            Point pointScreen = MousePosition;
            var mode = HitTest(PointToClient(pointScreen));
            if (mode != Win32.User32.HitTestValues.HTCLIENT)
            {
                is_resizable = true;
                SetCursorHit(mode);
                if (OperatingSystem.IsWindows())
                {
                    Win32.User32.ReleaseCapture();
                    Win32.User32.SendMessage(Handle, Win32.User32.WindowMessage.WM_NCLBUTTONDOWN, mode, Win32.Macros.MAKELPARAM(pointScreen.X, pointScreen.Y));
                }
                // As in DraggableMouseDown: the same gesture, expressed through the windowing layer
                // rather than by faking a non-client mouse-down message.
                else if (EdgeOf(mode) is { } edge) BeginResizeDrag(edge);
                is_resizable = false;
                return true;
            }
            return false;
        }

        #endregion

        #region 鼠标

        /// <summary>
        /// 鼠标拖拽大小使能
        /// </summary>
        [Description("鼠标拖拽大小使能"), Category(nameof(CategoryAttribute.Action)), DefaultValue(true)]
        public bool EnableHitTest { get; set; } = true;
        internal Win32.User32.HitTestValues HitTest(Point point)
        {
            if (Window.CanHandMessage && EnableHitTest)
            {
                float htSize = 8F * Dpi, htSize2 = htSize * 2;
                if (!OperatingSystem.IsWindows()) return Win32.User32.HitTestValues.HTCLIENT;
                Win32.User32.GetWindowRect(Handle, out var lpRect);

                var rect = new Rectangle(Point.Empty, lpRect.Size);

                var hitTestValue = Win32.User32.HitTestValues.HTCLIENT;
                var x = point.X;
                var y = point.Y;

                if (x < rect.Left + htSize2 && y < rect.Top + htSize2) hitTestValue = Win32.User32.HitTestValues.HTTOPLEFT;
                else if (x >= rect.Left + htSize2 && x <= rect.Right - htSize2 && y <= rect.Top + htSize) hitTestValue = Win32.User32.HitTestValues.HTTOP;
                else if (x > rect.Right - htSize2 && y <= rect.Top + htSize2) hitTestValue = Win32.User32.HitTestValues.HTTOPRIGHT;
                else if (x <= rect.Left + htSize && y >= rect.Top + htSize2 && y <= rect.Bottom - htSize2) hitTestValue = Win32.User32.HitTestValues.HTLEFT;
                else if (x >= rect.Right - htSize && y >= rect.Top * 2 + htSize && y <= rect.Bottom - htSize2) hitTestValue = Win32.User32.HitTestValues.HTRIGHT;
                else if (x <= rect.Left + htSize2 && y >= rect.Bottom - htSize2) hitTestValue = Win32.User32.HitTestValues.HTBOTTOMLEFT;
                else if (x > rect.Left + htSize2 && x < rect.Right - htSize2 && y >= rect.Bottom - htSize) hitTestValue = Win32.User32.HitTestValues.HTBOTTOM;
                else if (x >= rect.Right - htSize2 && y >= rect.Bottom - htSize2) hitTestValue = Win32.User32.HitTestValues.HTBOTTOMRIGHT;

                return hitTestValue;
            }
            else return Win32.User32.HitTestValues.HTCLIENT;
        }

        internal void SetCursorHit(Win32.User32.HitTestValues mode)
        {
            switch (mode)
            {
                case Win32.User32.HitTestValues.HTTOP:
                case Win32.User32.HitTestValues.HTBOTTOM:
                    LoadCursors(32645);
                    break;
                case Win32.User32.HitTestValues.HTLEFT:
                case Win32.User32.HitTestValues.HTRIGHT:
                    LoadCursors(32644);
                    break;
                case Win32.User32.HitTestValues.HTTOPLEFT:
                case Win32.User32.HitTestValues.HTBOTTOMRIGHT:
                    LoadCursors(32642);
                    break;
                case Win32.User32.HitTestValues.HTTOPRIGHT:
                case Win32.User32.HitTestValues.HTBOTTOMLEFT:
                    LoadCursors(32643);
                    break;
            }
        }

        internal void LoadCursors(int id)
        {
            // The OCR_* ids below are Win32 system cursors; off Windows there is no handle to load, and
            // SetCursorHit has already assigned the equivalent Cursors.* value.
            if (!OperatingSystem.IsWindows()) return;
            var handle = Win32.User32.LoadCursor(IntPtr.Zero, id);
            if (handle == IntPtr.Zero) return;
            Win32.User32.SetCursor(handle);
        }

        /// <summary>
        /// Maps a Win32 hit-test value to the window edge being dragged, for the managed resize path.
        /// Null where the hit is not a resizable edge.
        /// </summary>
        static Majorsilence.Forms.Backends.WindowEdge? EdgeOf(Win32.User32.HitTestValues mode) => mode switch
        {
            Win32.User32.HitTestValues.HTTOP => Majorsilence.Forms.Backends.WindowEdge.North,
            Win32.User32.HitTestValues.HTBOTTOM => Majorsilence.Forms.Backends.WindowEdge.South,
            Win32.User32.HitTestValues.HTLEFT => Majorsilence.Forms.Backends.WindowEdge.West,
            Win32.User32.HitTestValues.HTRIGHT => Majorsilence.Forms.Backends.WindowEdge.East,
            Win32.User32.HitTestValues.HTTOPLEFT => Majorsilence.Forms.Backends.WindowEdge.NorthWest,
            Win32.User32.HitTestValues.HTTOPRIGHT => Majorsilence.Forms.Backends.WindowEdge.NorthEast,
            Win32.User32.HitTestValues.HTBOTTOMLEFT => Majorsilence.Forms.Backends.WindowEdge.SouthWest,
            Win32.User32.HitTestValues.HTBOTTOMRIGHT => Majorsilence.Forms.Backends.WindowEdge.SouthEast,
            _ => null,
        };

        #endregion

        #endregion

        #region 按钮点击

        internal Action? ONESC;
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ONESC == null) return base.ProcessDialogKey(keyData);
            if ((keyData & (Keys.Alt | Keys.Control)) == Keys.None)
            {
                Keys keyCode = keyData & Keys.KeyCode;
                switch (keyCode)
                {
                    case Keys.Escape:
                        ONESC();
                        return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        #endregion

        #region 委托

#if NET40 || NET46 || NET48

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IAsyncResult BeginInvoke(Action method) => BeginInvoke(method, null);

        public void Invoke(Action method) => _ = Invoke(method, null);
        public T Invoke<T>(Func<T> method) => (T)Invoke(method, null);

#endif

        #endregion
    }
}