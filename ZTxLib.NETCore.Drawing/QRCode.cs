using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;

namespace ZTxLib.NETCore.Drawing
{
    /// <summary>
    /// QR二维码
    /// </summary>
    public class QRCode
    {
        private readonly QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.L);
        private readonly Brush darkBrush, lightBrush;
        private readonly Graphics graphics;

        /// <summary>
        /// 生成二维码绘制对象
        /// </summary>
        /// <param name="graphics">GD绘图图面I+</param>
        /// <param name="darkBrush">前景色</param>
        /// <param name="lightBrush">背景色</param>
        public QRCode(Graphics graphics, Brush darkBrush = null, Brush lightBrush = null)
        {
            if (darkBrush == null) darkBrush = Brushes.Black;
            if (lightBrush == null) lightBrush = Brushes.Transparent;
            this.graphics = graphics;
            this.darkBrush = darkBrush;
            this.lightBrush = lightBrush;
        }

        /// <summary>
        /// 开始绘制（仅覆盖）
        /// </summary>
        /// <param name="text">内容文字</param>
        /// <param name="pictureSize">图片大小</param>
        public void Draw(string text, int pictureSize)
        {
            QrCode qrCode = qrEncoder.Encode(text);
            FixedModuleSize moduleSize = new FixedModuleSize(pictureSize, QuietZoneModules.Zero);
            GraphicsRenderer render = new GraphicsRenderer(moduleSize, darkBrush, lightBrush);
            render.Draw(graphics, qrCode.Matrix);
        }

        /// <summary>
        /// 生成指定大小的位图二维码
        /// </summary>
        /// <param name="text">内容文字</param>
        /// <param name="pictureSize">图片大小</param>
        /// <returns></returns>
        public static Bitmap CreateBmp(string text, int pictureSize)
        {
            Bitmap bmp = new Bitmap(pictureSize * 22, pictureSize * 22);
            QRCode qrCode = new QRCode(Graphics.FromImage(bmp));
            qrCode.Draw(text, pictureSize);
            return bmp;
        }
    }
}