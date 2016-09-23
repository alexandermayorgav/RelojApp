using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceAFIS.Simple;
using Suprema;
using System.Windows.Forms;
namespace Logica
{
    public class ClsEscanner : Form
    {
        const int MAX_TEMPLATE_SIZE = 384;
        UFS_STATUS ufs_res;
        UFScannerManager ScannerManager;
        public UFScanner scanner { get; set; }


        public void iniciarScanner()
        {
            this.ScannerManager = new UFScannerManager(this);
            this.ufs_res = this.ScannerManager.Init();
            //get firts scanner
            this.scanner = this.ScannerManager.Scanners[0];
            setParametros();
        }

        public ClsRetorno getImage(String ruta, Huella.Dedo tipoDedo)
        {
            byte[] template = new byte[MAX_TEMPLATE_SIZE];
            int templateSize;
            int enrollQuallity;

            this.ufs_res = this.scanner.ClearCaptureImageBuffer();
            this.ufs_res = this.scanner.CaptureSingleImage();
            this.ufs_res = this.scanner.Extract(template, out templateSize, out enrollQuallity);

            ruta += "_" + ((int)tipoDedo).ToString() + ".bmp";
            this.ufs_res = this.scanner.SaveCaptureImageBufferToBMP(ruta);
            return new ClsRetorno(enrollQuallity, ruta, template);
        }

        private void setParametros()
        {
            this.scanner.Timeout = 5000;
            this.scanner.TemplateSize = MAX_TEMPLATE_SIZE;
            this.scanner.DetectCore = false;
        }

        public void apagarScanner()
        {
            this.ufs_res = this.ScannerManager.Uninit();
        }

    }

    public class ClsRetorno
    {
        public ClsRetorno(int calidad, String ruta, byte[] template)
        {
            this.calidad = calidad;
            this.ruta = ruta;
            this.template = template;
        }
        public int calidad { get; set; }
        public String ruta { get; set; }
        public byte[] template { get; set; }

    }
}
