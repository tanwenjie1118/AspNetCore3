﻿
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTest.Import
{
    public class ExcelTests
    {
        public IEnumerable<EnumTemplate> GetEnumTemplates(string path, string ename)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo newFile = new FileInfo(path);
            if (!newFile.Exists)
            {
                throw new Exception("file not exist");
            }

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                if (package.Workbook.Worksheets.Count > 0)
                {
                    foreach (var sheet in package.Workbook.Worksheets)
                    {
                        var sName = sheet.Name;

                        if (sName != ename)
                        {
                            continue;
                        }

                        if (sheet.Cells.Count() > 0)
                        {
                            var startRow = 2;
                            var startCol = 1;
                            foreach (var cell in sheet.Cells)
                            {
                                var nameEnglish = sheet.Cells[startRow, startCol++].Text;
                                var nameChinese = sheet.Cells[startRow, startCol++].Text;
                                var index = sheet.Cells[startRow, startCol++].Text;

                                startRow++;
                                startCol = 1;

                                if (string.IsNullOrWhiteSpace(nameEnglish))
                                {
                                    continue;
                                }

                                var model = new EnumTemplate()
                                {
                                    EnglishName = nameEnglish,
                                    ChineseName = nameChinese,
                                    Index = index
                                };

                                yield return model;
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<ModelTemplate> GetModelTemplates(string path, string modelname)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo newFile = new FileInfo(path);
            if (!newFile.Exists)
            {
                throw new System.Exception("file not exist");
            }

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                if (package.Workbook.Worksheets.Count > 0)
                {
                    foreach (var sheet in package.Workbook.Worksheets)
                    {
                        var className = sheet.Name;

                        if (modelname != className)
                        {
                            continue;
                        }

                        if (sheet.Cells.Count() > 0)
                        {
                            var startRow = 2;
                            var startCol = 1;
                            foreach (var cell in sheet.Cells)
                            {
                                var nameEnglish = sheet.Cells[startRow, startCol++].Text;
                                var nameChinese = sheet.Cells[startRow, startCol++].Text;
                                var remark = sheet.Cells[startRow, startCol++].Text;
                                var type = TransferStandardType(sheet.Cells[startRow, startCol++].Text);
                                var length = sheet.Cells[startRow, startCol++].Text;
                                var isRequired = sheet.Cells[startRow, startCol++].Text;
                                var key = sheet.Cells[startRow, startCol].Text;

                                startRow++;
                                startCol = 1;

                                if (string.IsNullOrWhiteSpace(nameEnglish))
                                {
                                    continue;
                                }

                                var model = new ModelTemplate()
                                {
                                    EnglishName = nameEnglish,
                                    ChineseName = nameChinese,
                                    IsRequired = isRequired,
                                    Length = length,
                                    Remark = remark,
                                    Type = type,
                                    Key = key
                                };

                                yield return model;
                            }
                        }
                    }
                }
            }
        }

        private string TransferStandardType(string type)
        {
            switch (type)
            {
                case var t when t.ToLower() == "int":
                    return "int";
                case var t when t.ToLower() == "string":
                    return "string";
                case var t when t.ToLower() == "datetime":
                    return "DateTime";
                case var t when t.ToLower() == "float":
                    return "float";
                case var t when t.ToLower() == "decimal":
                    return "decimal";
                case var t when t.ToLower() == "bool":
                    return "bool";
                case var t when t.ToLower() == "list<int>":
                    return "List<int>";
                case var t when t.ToLower() == "list<string>":
                    return "List<string>";
                default:
                    return string.Empty;
            }
        }

        public class ModelTemplate
        {
            public string EnglishName { set; get; }
            public string ChineseName { set; get; }
            public string Remark { set; get; }
            public string Type { set; get; }
            public string Length { set; get; }
            public string IsRequired { set; get; }
            public string Key { set; get; }
        }

        public class EnumTemplate
        {
            public string EnglishName { set; get; }
            public string ChineseName { set; get; }
            public string Index { set; get; }
        }
    }

}
