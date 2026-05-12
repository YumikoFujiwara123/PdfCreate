using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfCreate
{
    public static class CsvLoader
    {
        public static List<T> LoadCsv<T>(string csvPath) where T : new()
        {
            var results = new List<T>();

            using var parser = new TextFieldParser(csvPath);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;

            if (parser.EndOfData)
                return results;

            string[]? headers = parser.ReadFields();
            if (headers == null || headers.Length == 0)
                return results;

            // ファイル形式のチェック（=headerが一致しているか）
            ValidateHeaderExactMatch<T>(headers);

            // csvファイルを１件づつ読み込む
            while (!parser.EndOfData)
            {
                string[]? fields = parser.ReadFields();
                if (fields == null)
                    continue;

                // タイトル名とdataの辞書をつくる
                var row = ToDictionary(headers, fields);

                // csvをクラスにデータ変換する
                var result = CreateObject<T>(row);
                results.Add(result);
            }

            return results;
        }

        private static T CreateObject<T>(Dictionary<string, string> row) where T : new()
        {
            var result = new T();

            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                if (row.TryGetValue(prop.Name, out var value))
                {
                    prop.SetValue(result, value);
                }
            }

            return result;
        }

        private static Dictionary<string, string> ToDictionary(string[] headers, string[] fields)
        {
            // 項目名は大文字／小文字を区別しない
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < headers.Length; i++)
            {
                var value = i < fields.Length ? fields[i] : "";
                dict[headers[i]] = value ?? "";
            }

            return dict;
        }

        private static void ValidateHeaderExactMatch<T>(string[] headers)
        {
            var props = typeof(T).GetProperties();

            // プロパティ名の配列（順番そのまま）
            var propNames = props.Select(p => p.Name).ToArray();

            // 件数チェック
            if (headers.Length != propNames.Length)
                throw new Exception("CSVの列数とクラスのプロパティ数が一致しません。");

            // 順番＋名前チェック
            for (int i = 0; i < headers.Length; i++)
            {
                // 項目名は大文字／小文字を区別しない
                if (!string.Equals(headers[i], propNames[i], StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception(
                        $"ヘッダ不一致: {i + 1}列目が違います。\n" +
                        $"CSV: {headers[i]}\n" +
                        $"クラス: {propNames[i]}"
                    );
                }
            }
        }

        private static string Get(Dictionary<string, string> row, string key)
        {
            return row.TryGetValue(key, out var value) ? value : "";
        }

        private static int GetInt(Dictionary<string, string> row, string key)
        {
            if (!row.TryGetValue(key, out var value))
                return 0;

            return int.TryParse(value, out var n) ? n : 0;
        }
    }
}
