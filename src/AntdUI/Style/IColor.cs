// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI.Theme
{
    public class IColor
    {
        public Color Primary => AntdUI.Style.Get(Colour.Primary);
        public Color PrimaryColor => AntdUI.Style.Get(Colour.PrimaryColor);
        public Color PrimaryHover => AntdUI.Style.Get(Colour.PrimaryHover);
        public Color PrimaryActive => AntdUI.Style.Get(Colour.PrimaryActive);
        public Color PrimaryBg => AntdUI.Style.Get(Colour.PrimaryBg);
        public Color PrimaryBgHover => AntdUI.Style.Get(Colour.PrimaryBgHover);
        public Color PrimaryBorder => AntdUI.Style.Get(Colour.PrimaryBorder);
        public Color PrimaryBorderHover => AntdUI.Style.Get(Colour.PrimaryBorderHover);

        [System.Obsolete("use AntdUI.Style.SetPrimary")]
        public void SetPrimary(Color primary) => AntdUI.Style.SetPrimary(primary);

        public Color Success => AntdUI.Style.Get(Colour.Success);
        public Color SuccessColor => AntdUI.Style.Get(Colour.SuccessColor);
        public Color SuccessBg => AntdUI.Style.Get(Colour.SuccessBg);
        public Color SuccessBorder => AntdUI.Style.Get(Colour.SuccessBorder);
        public Color SuccessHover => AntdUI.Style.Get(Colour.SuccessHover);
        public Color SuccessActive => AntdUI.Style.Get(Colour.SuccessActive);

        [System.Obsolete("use AntdUI.Style.SetSuccess")]
        public void SetSuccess(Color success) => AntdUI.Style.SetSuccess(success);

        public Color Warning => AntdUI.Style.Get(Colour.Warning);
        public Color WarningColor => AntdUI.Style.Get(Colour.WarningColor);
        public Color WarningBg => AntdUI.Style.Get(Colour.WarningBg);
        public Color WarningBorder => AntdUI.Style.Get(Colour.WarningBorder);
        public Color WarningHover => AntdUI.Style.Get(Colour.WarningHover);
        public Color WarningActive => AntdUI.Style.Get(Colour.WarningActive);

        [System.Obsolete("use AntdUI.Style.SetWarning")]
        public void SetWarning(Color warning) => AntdUI.Style.SetWarning(warning);

        public Color Error => AntdUI.Style.Get(Colour.Error);
        public Color ErrorColor => AntdUI.Style.Get(Colour.ErrorColor);
        public Color ErrorBg => AntdUI.Style.Get(Colour.ErrorBg);
        public Color ErrorBorder => AntdUI.Style.Get(Colour.ErrorBorder);
        public Color ErrorHover => AntdUI.Style.Get(Colour.ErrorHover);
        public Color ErrorActive => AntdUI.Style.Get(Colour.ErrorActive);

        [System.Obsolete("use AntdUI.Style.SetError")]
        public void SetError(Color error) => AntdUI.Style.SetError(error);

        public Color Info => AntdUI.Style.Get(Colour.Info);
        public Color InfoColor => AntdUI.Style.Get(Colour.InfoColor);
        public Color InfoBg => AntdUI.Style.Get(Colour.InfoBg);
        public Color InfoBorder => AntdUI.Style.Get(Colour.InfoBorder);
        public Color InfoHover => AntdUI.Style.Get(Colour.InfoHover);
        public Color InfoActive => AntdUI.Style.Get(Colour.InfoActive);

        [System.Obsolete("use AntdUI.Style.SetInfo")]
        public void SetInfo(Color info) => AntdUI.Style.SetInfo(info);

        public Color DefaultBg => AntdUI.Style.Get(Colour.DefaultBg);
        public Color DefaultColor => AntdUI.Style.Get(Colour.DefaultColor);
        public Color DefaultBorder => AntdUI.Style.Get(Colour.DefaultBorder);

        public Color TagDefaultBg => AntdUI.Style.Get(Colour.TagDefaultBg);
        public Color TagDefaultColor => AntdUI.Style.Get(Colour.TagDefaultColor);

        public Color TextBase => AntdUI.Style.Get(Colour.TextBase);
        public Color Text => AntdUI.Style.Get(Colour.Text);
        public Color TextSecondary => AntdUI.Style.Get(Colour.TextSecondary);
        public Color TextTertiary => AntdUI.Style.Get(Colour.TextTertiary);
        public Color TextQuaternary => AntdUI.Style.Get(Colour.TextQuaternary);

        public Color BgBase => AntdUI.Style.Get(Colour.BgBase);
        public Color BgContainer => AntdUI.Style.Get(Colour.BgContainer);
        public Color BgElevated => AntdUI.Style.Get(Colour.BgElevated);
        public Color BgLayout => AntdUI.Style.Get(Colour.BgLayout);

        public Color Fill => AntdUI.Style.Get(Colour.Fill);
        public Color FillSecondary => AntdUI.Style.Get(Colour.FillSecondary);
        public Color FillTertiary => AntdUI.Style.Get(Colour.FillTertiary);
        public Color FillQuaternary => AntdUI.Style.Get(Colour.FillQuaternary);

        public Color BorderColor => AntdUI.Style.Get(Colour.BorderColor);
        public Color BorderSecondary => AntdUI.Style.Get(Colour.BorderSecondary);

        public Color BorderColorDisable => AntdUI.Style.Get(Colour.BorderColorDisable);

        public Color Split => AntdUI.Style.Get(Colour.Split);

        public Color HoverBg => AntdUI.Style.Get(Colour.HoverBg);

        public Color HoverColor => AntdUI.Style.Get(Colour.HoverColor);

        public Color SliderHandleColorDisabled => AntdUI.Style.Get(Colour.SliderHandleColorDisabled);

        public Color TextSpotlight => AntdUI.Style.Get(Colour.TextSpotlight);

        public Color BgSpotlight => AntdUI.Style.Get(Colour.BgSpotlight);

        public Color SwitchHandleBg => AntdUI.Style.Get(Colour.SwitchHandleBg);
    }
}