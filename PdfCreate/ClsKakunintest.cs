using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using System.Data.Common;

namespace PdfCreate
{
    public class ClsKakunintest : IDocument
    {
        // 個人別成績表出力用クラス
        private Kakunintest _seiseki;

        public ClsKakunintest(Kakunintest seiseki)
        {
            _seiseki = seiseki;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {

            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);  // 適宜余白　単位はポイント            

                page.DefaultTextStyle(x => x.FontSize(10));

                page.Content()
                    .Column(column =>
                    {

                        // 個人情報（ここを丸ごと置き換え）
                        column.Item().Element(container =>
                        {
                            container.Layers(layers =>
                            {
                                // =========================
                                // ① 右側：罫線テーブル
                                // =========================
                                layers.PrimaryLayer().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(18);
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(8);
                                        columns.RelativeColumn(1);
                                    });

                                    // タイトル
                                    table.Cell().Row(1).Column(2).ColumnSpan(3)
                                        .Text("●●●●●")
                                        .Bold()
                                        .FontSize(10)
                                        .AlignLeft();

                                    // 左列は空白にする（重要）
                                    table.Cell().Row(2).Column(1).Element(c => c.Border(0).Text(""));
                                    table.Cell().Row(3).Column(1).Element(c => c.Border(0).Text(""));
                                    table.Cell().Row(4).Column(1).Element(c => c.Border(0).Text(""));
                                    table.Cell().Row(5).Column(1).Element(c => c.Border(0).Text(""));
                                    table.Cell().Row(6).Column(1).Element(c => c.Border(0).Text(""));

                                });

                                // =========================
                                // ② 左側：住所（絶対位置）
                                // =========================
                                layers.Layer().Element(c =>
                                {
                                    c.PaddingTop(0)
                                     .PaddingLeft(35)
                                     .Width(250)
                                     .Column(left =>
                                     {
                                         left.Item().Text(_seiseki.Yubin);
                                         left.Item().Text(_seiseki.Address1);
                                         left.Item().Text(_seiseki.Address2);
                                         left.Item().Text(_seiseki.Name + "　様");
                                     });
                                });
                            });
                        });



                        string wkGakuid = string.Empty;
                        wkGakuid = _seiseki.Jno;

                        column.Item().PaddingLeft(80).Text(wkGakuid.PadLeft(42)).Bold().FontSize(10);

                        column.Item().Height(130);      // 余白


                        column.Item().Text("○○○テスト　個人別成績表").FontFamily("MS PGothic").FontSize(16).AlignCenter();

                        column.Item().Text("-------------------------------------------------------------------------------------------------------------").Bold().FontSize(12);
                        //container.LineHorizontal(1);

                        // 総合評価
                        column.Item().Element(cell =>
                        {
                            cell.Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(3);
                                });

                                table.Cell().Row(1).Column(1).Element(c => c.Background(Colors.Grey.Lighten2).Border(1).AlignCenter().AlignMiddle().Text(""));
                                table.Cell().Row(1).Column(2).Element(c => c.Background(Colors.Grey.Lighten2).Border(1).AlignCenter().Text("科目１").FontFamily("MS PGothic"));
                                table.Cell().Row(1).Column(3).Element(c => c.Background(Colors.Grey.Lighten2).Border(1).AlignCenter().Text("科目２").FontFamily("MS PGothic"));
                                table.Cell().Row(1).Column(4).Element(c => c.Background(Colors.Grey.Lighten2).Border(1).AlignCenter().Text("科目３").FontFamily("MS PGothic"));
                                table.Cell().Row(1).Column(5).Element(c => c.Background(Colors.Grey.Lighten2).Border(1).AlignCenter().Text("科目４").FontFamily("MS PGothic"));
                                table.Cell().Row(1).Column(6).Element(c => c.Background(Colors.Grey.Lighten2).Border(1).AlignCenter().Text("科目５ ").FontFamily("MS PGothic"));
                                table.Cell().Row(1).Column(7).Element(c => c.Background(Colors.Grey.Lighten2).Border(1).AlignCenter().Text("合計 ").FontFamily("MS PGothic"));
                                table.Cell().Row(1).Column(8).Element(c => c.Background(Colors.Grey.Lighten2).Border(1).AlignCenter().Text("順位 ").FontFamily("MS PGothic"));


                                table.Cell().Row(2).Column(1).Element(c => c.Background(Colors.Grey.Lighten2).Border(0.5f).AlignCenter().AlignMiddle().Text("選択"));

                                table.Cell().Row(2).Column(2).Element(c => c.Border(0.5f).AlignCenter().AlignMiddle().Text(_seiseki.Kamoku1).Bold().FontSize(10));
                                table.Cell().Row(2).Column(3).Element(c => c.Border(0.5f).AlignCenter().AlignMiddle().Text(_seiseki.Kamoku2).Bold().FontSize(10));
                                table.Cell().Row(2).Column(4).Element(c => c.Border(0.5f).AlignCenter().AlignMiddle().Text(_seiseki.Kamoku3).Bold().FontSize(10));
                                table.Cell().Row(2).Column(5).Element(c => c.Border(0.5f).AlignCenter().AlignMiddle().Text(_seiseki.Kamoku4).Bold().FontSize(10));
                                table.Cell().Row(2).Column(6).Element(c => c.Border(0.5f).AlignCenter().AlignMiddle().Text(_seiseki.Kamoku5).Bold().FontSize(10));
                                table.Cell().Row(2).Column(7).Element(c => c.Border(0.5f).AlignCenter().AlignMiddle().Text(_seiseki.Goukei).Bold().FontSize(10));
                                table.Cell().Row(2).Column(8).Element(c => c.Border(0.5f).AlignCenter().AlignMiddle().Text(_seiseki.Junni).Bold().FontSize(10));


                            });
                        });

                        //column.Item().Text("-------------------------------------------------------------------------------------------------------------------").Bold().FontSize(12);
                        column.Item().Height(5);

                        column.Item().PaddingVertical(2).Text("総評").FontSize(10).FontFamily("MS PGothic");

                        if (int.TryParse(_seiseki.Goukei, out int goukei))
                        {
                            switch (goukei)
                            {
                                case >= 0 and < 150:
                                    column.Item()
                                     .Border(1)
                                     .Padding(8)
                                     .MinHeight(65)
                                     .Text("これからです！（＞＿＜）ノ")
                                     .FontFamily("MS PMincho")
                                     .FontSize(10);
                                    break;
                                case >= 150 and < 380:
                                    column.Item()
                                    .Border(1)
                                    .Padding(8)
                                    .MinHeight(65)
                                    .Text("もう少しがんばんりましょう（・＿・）")
                                    .FontFamily("MS PMincho")
                                    .FontSize(10);
                                    break;
                                case >= 380:
                                    column.Item()
                                    .Border(1)
                                    .Padding(8)
                                    .MinHeight(65)
                                    //.Text("もうちょっとです！(｀・ω・´)ノ")
                                    .Text("がんばりました！＼(^o^)／")
                                    .FontFamily("MS PMincho")
                                    .FontSize(10);
                                    break;
                                default:
                                    column.Item()
                                    .Border(1)
                                    .Padding(8)
                                    .MinHeight(65)
                                    .Text("あなたは何点ですか？？")
                                    .FontFamily("MS PMincho")
                                    .FontSize(10);
                                    break;

                            }
                        }

                    });
            });
        }
    }
}