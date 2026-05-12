using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using QuestPDF.Drawing;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace PdfCreate
{
    public partial class FormMain : Form
    {
        public readonly string[] _args;

        private List<Kakunintest> _kakuninresults = new();

        private const string TEMP_PDF = "TempPdf";

        public FormMain() : this(Array.Empty<string>())
        {
            // 引数がない時のための処理
            // この後 FormMain(string[] args)  が実行されるので、
            // ここにはInitializeComponentは不要
        }
        public FormMain(string[] args) 
        {
            InitializeComponent();

            _args = args ?? Array.Empty<string>();

            txtCsvPath.AllowDrop = true;
            txtCsvPath.DragEnter += txtCsvPath_DragEnter;
            txtCsvPath.DragDrop += txtCsvPath_DragDrop;

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                //自分自身のバージョン情報を取得する
                var exePath = Application.ExecutablePath;
                var ver = FileVersionInfo.GetVersionInfo(exePath);
                lblVersion.Text = "Ver." + ver.FileVersion;
            }
            catch
            {
                lblVersion.Text = "Ver.-";
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_args.Length > 0)   //引数ありで他のアプリから起動された場合に実行
            {

                var csvPath = _args[0];

                txtCsvPath.Enabled = false;
                optJunihyou.Enabled = false;
                optKojinbetu.Enabled = false;
                label1.Visible = false;

                // _args[0] :成績データ（csv)、_args[1] :帳票種類　1：個人別成績表、2：成績順位表
                if (_args.Length > 1 && int.TryParse(_args[1], out var type))
                {
                    switch (type)
                    {
                        case 1:     // 個人別成績表
                            optKojinbetu.Checked = true;
                            break;
                        case 2:     // 成績順位表
                            optJunihyou.Checked = true;
                            break;
                        default:
                            MessageBox.Show("帳票種別が不正です");
                            return;
                    }
                }

                if (File.Exists(csvPath))
                {
                    txtCsvPath.Text = csvPath;

                    try
                    {
                        LoadCsvAndRun(csvPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("自動実行エラー: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("CSVファイルが見つかりません: " + csvPath);
                }
            }

        }

        private void LoadCsvAndRun(string csvPath)
        {
            // csvファイルからのデータ取得
            _kakuninresults = CsvLoader.LoadCsv<Kakunintest>(csvPath);
            //MessageBox.Show($"{_kakuninresults.Count}件 読み込みました。");
         
            // PDF作成
            cmdCreatePdf.PerformClick();
        }

        #region "■ファイルのドラッグアンドドロップの設定"

        private void txtCsvPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void txtCsvPath_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;

            var files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length == 0) return;

            var csvPath = files[0];
            txtCsvPath.Text = csvPath;

            try
            {
                _kakuninresults = CsvLoader.LoadCsv<Kakunintest>(csvPath);
                MessageBox.Show($"{_kakuninresults.Count}件 読み込みました。");

            }
            catch (Exception ex)
            {
                MessageBox.Show("CSV読込エラー: " + ex.Message);
            }
        }

        #endregion

        private async void cmdCreatePdf_Click(object sender, EventArgs e)
        {
            // 成績データ格納場所
            //      C:\Users\ユーザー名\AppData\Local\TempPdf
            // 成績表（PDF）格納場所
            //      C:\Users\ユーザー名\AppData\Local\TempPdf\Pdf
            string pdfPath = string.Empty;

            string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string tempDir = Path.Combine(baseDir, TEMP_PDF);
            string pdfDir = Path.Combine(tempDir, "Pdf");

            string outputFolder = string.Empty;

            var dt = DateTime.Now;

            // pdfDirフォルダが既に存在する場合、一旦フォルダごと削除して再作成する（フォルダのクリア）
            if(Directory.Exists(pdfDir))
            {
                Directory.Delete(pdfDir, true);
                Directory.CreateDirectory(pdfDir);
            }
            else
            {
                Directory.CreateDirectory(pdfDir);
            }
 
            cmdCreatePdf.Enabled = false;

            try
            {
                if (optKojinbetu.Checked)
                {
                    await CreateKojinbetuPdfAsync(pdfDir);
                }
                else if (optJunihyou.Checked)
                {
                    await CreateJunnihyouPdfAsync(pdfDir);
                }
          
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF作成エラー: " + ex.Message);

                //  最終結果：失敗　(他のアプリから呼び出された場合は、リターンコードを設定する）
                if (_args.Length > 0)
                {
                    Environment.Exit(1);
                    Application.Exit();
                }
            }
            finally
            {
                cmdCreatePdf.Enabled = true;
            }
        }

        private async Task CreateKojinbetuPdfAsync(string outputFolder)
        {
            //--------------------
            // 個人別成績表
            //--------------------
            if (_kakuninresults == null || _kakuninresults.Count == 0)
            {
                MessageBox.Show("先にCSVを読み込んでください。");
                return;
            }

            var dt = DateTime.Now;
            var mergeFile = Path.Combine(outputFolder, $"{dt:yyyyMMddHHmmss}_個人別成績表ALL.pdf");

            progressBar1.Minimum = 0;
            progressBar1.Maximum = _kakuninresults.Count;
            progressBar1.Value = 0;
            lblStatus.Text = $"0 / {_kakuninresults.Count} 件";

            int count = 0;
            string kind = "0";

            foreach (var r in _kakuninresults)  
            {
                try
                {
                    var safeName = SanitizeFileName(r.Name);
                    var safeNo = r.Jno ?? "";

                    var fileName = BuildPdfFileName(outputFolder, kind, safeNo, safeName);

                    await Task.Run(() =>
                    {
                        var doc = new ClsKakunintest(r);
                        doc.GeneratePdf(fileName);
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ID:{r.Jno}\r\n{ex.Message}");
                }

                count++;
                UpdateProgress(count, _kakuninresults.Count);
            }
                     
        }

        private async Task CreateJunnihyouPdfAsync(string outputFolder)
        {
            //--------------------
            // 成績順位表
            //--------------------
            if (_kakuninresults == null || _kakuninresults.Count == 0)
            {
                MessageBox.Show("先にCSVを読み込んでください。");
                return;
            }

            var dt = DateTime.Now;
            var mergeFile = Path.Combine(outputFolder, $"{dt:yyyyMMddHHmmss}_成績順位表.pdf");

            progressBar1.Minimum = 0;
            progressBar1.Maximum = _kakuninresults.Count;
            progressBar1.Value = 0;
            lblStatus.Text = $"0 / {_kakuninresults.Count} 件";

            int count = 0;


            string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string tempDir = Path.Combine(baseDir, TEMP_PDF);
            string pdfDir = Path.Combine(tempDir, "Pdf");
            // string outputFolder;
            outputFolder = pdfDir;
            var fileName2 = Path.Combine(outputFolder, $"成績順位表.pdf");

            await Task.Run(() =>
            {
                var doc2 = new ClsRankingList(_kakuninresults);
                doc2.GeneratePdf(fileName2);
            });

            MessageBox.Show("End！");

             Process.Start(new ProcessStartInfo
             {
                 FileName = fileName2,
                 UseShellExecute = true
             });

        }

        private void MergePdfFiles(string inputFolder, string outputFile,int seq)
    {
        var comparer = StringComparer.Create(new CultureInfo("ja-JP"), ignoreCase: false);

        List<string> files;

        switch (seq)
        {
            case 1:         // 50音順
                files = Directory.GetFiles(inputFolder, "*.pdf")
                    .Where(f => Path.GetFileName(f) != Path.GetFileName(outputFile))
                    .OrderBy(f => Path.GetFileNameWithoutExtension(f), comparer)
                    .ToList();
                break;
            case 2:         // 受験番号順
                files = Directory.GetFiles(inputFolder, "*.pdf")
                    .Where(f => Path.GetFileName(f) != Path.GetFileName(outputFile)) // 出力先自身は除外
                    .OrderBy(f =>
                    {
                        var name = Path.GetFileNameWithoutExtension(f);
                        var parts = name.Split('_');

                        return (parts.Length > 0 && int.TryParse(parts[0], out var studentNo)) ? studentNo : int.MaxValue;
                    })
                    .ThenBy(f => f)     // 同一番号時の保険
                    .ToList();
                break;
            case 3:         // 学校・クラス・50音順                    
                files = Directory.GetFiles(inputFolder, "*.pdf")
                    .Where(f => Path.GetFileName(f) != Path.GetFileName(outputFile))
                    .OrderBy(f =>
                    {
                        var parts = Path.GetFileNameWithoutExtension(f).Split('_');
                        return (parts.Length > 0 && int.TryParse(parts[0], out var n1))
                            ? n1
                            : int.MaxValue;
                    })
                    .ThenBy(f =>
                    {
                        var parts = Path.GetFileNameWithoutExtension(f).Split('_');
                        return (parts.Length > 1 && int.TryParse(parts[1], out var n2))
                            ? n2
                            : int.MaxValue;
                    })
                    .ThenBy(f =>
                    {
                        var parts = Path.GetFileNameWithoutExtension(f).Split('_');
                        return parts.Length > 2 ? parts[2] : "";
                    }, comparer)
                    .ToList();
                break;
            default:
                throw new ArgumentException("seq の値が不正です。");
        }

        if (files.Count == 0)
            throw new InvalidOperationException("結合対象のPDFがありません。");

        using var outputDocument = new PdfDocument();

        foreach (var file in files)
        {
            using var inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

            for (int i = 0; i < inputDocument.PageCount; i++)
            {
                // Merge処理
                outputDocument.AddPage(inputDocument.Pages[i]);
            }
        }

        outputDocument.Save(outputFile);

        // Merge後のファイル削除（お掃除！）
        foreach (var file in files)
        {
            try
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch (Exception ex)
            {
                // ログ出す or 無視
                Console.WriteLine($"削除失敗: {file} - {ex.Message}");
            }
        }
    }

        private string BuildPdfFileName(
            string outputFolder,
            string seq,
            string safeNo,
            string safeName )
            {
                switch (seq)
                {
                    case "1": return Path.Combine(outputFolder, $"{safeNo}_{safeName}.pdf");
                    case "2": return Path.Combine(outputFolder, $"成績順位表.pdf");
                    default: return Path.Combine(outputFolder, $"{safeNo}_{safeName}.pdf");
                }
            }

        private void UpdateProgress(int count, int total)
        {
            progressBar1.Value = count;
            lblStatus.Text = $"{count} / {total} 件";
        }

        private string SanitizeFileName(string? value)
        { 
            // ファイル名として使えない文字がないかをチェックし、該当の文字があれば""へ置き換える
            var result = value ?? ""; 
            foreach (var c in Path.GetInvalidFileNameChars())
                result = result.Replace(c.ToString(), ""); 
            return result; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var outputFolder = @"C:\Temp\PdfOut";

            //var mergedFile = Path.Combine(outputFolder, "0000_成績表まとめ.pdf");

            //MergePdfFiles(outputFolder, mergedFile);

            MessageBox.Show("end!");
        }

   
    }
}
