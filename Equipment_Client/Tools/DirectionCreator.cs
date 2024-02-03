using Equipment_Client.Models;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;





namespace Equipment_Client.Tools
{
    public class DirectionCreator
    {
        public static Repair Repair { get; set; }
        public static ReplacementOfConsumable ReplacementOfConsumable { get; set; }
        public static ObservableCollection<byte[]> ImagesResponsable { get; set; } = new();
        public static ObservableCollection<byte[]> Images { get; set; } = new();
        public static void GetDirections(Report report)
        {
            Repair = report.Repairs.FirstOrDefault();
            ReplacementOfConsumable = report.ReplacementOfConsumables.FirstOrDefault();

            foreach (var image in report.FhotoPaths.Where(s => s.IdScientist == report.
                    IdBookingNavigation.IdScientistNavigation.Id))
            {
                Images.Add(image.Fhoto);
            }
            foreach (var image in report.FhotoPaths.Where(s => s.IdScientist == report.
                    IdBookingNavigation.IdEquipmentNavigation.IdReponsibleScientistsNavigation.Id))
            {
                ImagesResponsable.Add(image.Fhoto);
            }

            Document document = new Document();
            document.LoadFromFile("Отчёт об использовании научного оборудования - Что-то очень крутое и значимое.docx");
            Section section = document.Sections[0];

            //Paragraph paragraphNameEquipment = section.Paragraphs[1];
           // paragraphNameEquipment.Replace("{nameEquipment}", $"{report.IdBookingNavigation.IdEquipmentNavigation.Name}", false, true);

            Table table1 = section.Tables[0] as Table;//название оборудования
            TableRow tableRow1 = table1.Rows[0];
            Paragraph paragraph1 = tableRow1.Cells[0].Paragraphs[0];
            TextRange textRangeNameEquipment = paragraph1.AppendText(report.IdBookingNavigation.IdEquipmentNavigation.Name);
            FormatText(textRangeNameEquipment);

            Table table2 = section.Tables[1] as Table;//состояние оборудования и фотки
            TableRow tableRow2_1 = table2.Rows[0];
            Paragraph paragraph2 = tableRow2_1.Cells[0].Paragraphs[0];
            TextRange textRangeCondition = paragraph2.AppendText(report.Condition);//cocтояние
            FormatText(textRangeCondition);
            TableRow tableRow2_2 = table2.Rows[1];
            DocPicture docPicture1;
            for(int i = 0; i < Images.Count; i++)
            {
                Paragraph p = tableRow2_2.Cells[i].Paragraphs[0];
                docPicture1 = p.AppendPicture(Images[i]);
                docPicture1.Width = 150;
                docPicture1.Height = 150;
            }

            Table table3 = section.Tables[2] as Table;//наличие ЗИП и расходных материалов
            TableRow tableRow3 = table3.Rows[1];
            Paragraph paragraph3_1 = tableRow3.Cells[0].Paragraphs[0];
            Paragraph paragraph3_2 = tableRow3.Cells[1].Paragraphs[0];
            TextRange textRangeAvailabilityZipOrAvailabilityСonsumable;
            if (report.AvailabilityZip == 1)
            {
                textRangeAvailabilityZipOrAvailabilityСonsumable = paragraph3_1.AppendText("Присутствует");
                FormatText(textRangeAvailabilityZipOrAvailabilityСonsumable);
            }
            else
            {
                textRangeAvailabilityZipOrAvailabilityСonsumable = paragraph3_1.AppendText("Отсутствует");
                FormatText(textRangeAvailabilityZipOrAvailabilityСonsumable);
            }
            if(report.AvailabilityСonsumable == 1)
            {
                textRangeAvailabilityZipOrAvailabilityСonsumable = paragraph3_2.AppendText("Присутствуют");
                FormatText(textRangeAvailabilityZipOrAvailabilityСonsumable);
            }
            else
            {
                textRangeAvailabilityZipOrAvailabilityСonsumable = paragraph3_2.AppendText("Отсутствуют");
                FormatText(textRangeAvailabilityZipOrAvailabilityСonsumable);
            }

            Table table4 = section.Tables[3] as Table;//Информация о ответственном исполнителе
            TableRow tableRow4_1 = table4.Rows[0];
            Paragraph paragraph4_1 = tableRow4_1.Cells[1].Paragraphs[0];
            TextRange textRangeDepartment = paragraph4_1.AppendText(report.IdBookingNavigation.IdScientistNavigation
                                                                    .IdLaboratotyNavigation.IdDepartmentNavigation.Title);//отдел
            TableRow tableRow4_2 = table4.Rows[1];
            Paragraph paragraph4_2 = tableRow4_2.Cells[1].Paragraphs[0];
            TextRange textRangeLaboratory = paragraph4_2.AppendText(report.IdBookingNavigation.IdScientistNavigation
                                                                    .IdLaboratotyNavigation.Title);//лаборатория
            TableRow tableRow4_3 = table4.Rows[2];
            Paragraph paragraph4_3 = tableRow4_3.Cells[1].Paragraphs[0];
            TextRange textRangeFIO = paragraph4_3.AppendText($"{report.IdBookingNavigation.IdScientistNavigation.Lastname}" +
                $" {report.IdBookingNavigation.IdScientistNavigation.Firstname} {report.IdBookingNavigation.IdScientistNavigation.Patronymic}");//ФИО
            TableRow tableRow4_4 = table4.Rows[3];
            Paragraph paragraph4_4 = tableRow4_4.Cells[1].Paragraphs[0];
            TextRange textRangeOperators = paragraph4_4.AppendText(report.Operators);//Операторы
            FormatText(textRangeDepartment);
            FormatText(textRangeLaboratory);
            FormatText(textRangeFIO);
            FormatText(textRangeOperators);

            Table table5 = section.Tables[4] as Table;//Время бронирования оборудования по плану
            TableRow tableRow5 = table5.Rows[0];
            Paragraph paragraph5_1 = tableRow5.Cells[0].Paragraphs[0];//начало 
            Paragraph paragraph5_2 = tableRow5.Cells[1].Paragraphs[0];//конец
            paragraph5_1.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            paragraph5_2.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            TextRange textRangeDateStart = paragraph5_1.AppendText(report.IdBookingNavigation.DateStart.ToShortDateString());
            TextRange textRangeDateEnd = paragraph5_2.AppendText(report.IdBookingNavigation.DateEnd.ToShortDateString());
            FormatText(textRangeDateStart);
            FormatText(textRangeDateEnd);
            

            Table table6 = section.Tables[5] as Table;//Время бронирования оборудования фактически
            TableRow tableRow6 = table6.Rows[0];
            Paragraph paragraph6_1 = tableRow6.Cells[0].Paragraphs[0];//начало 
            Paragraph paragraph6_2 = tableRow6.Cells[1].Paragraphs[0];//конец
            paragraph6_1.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            paragraph6_2.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            TextRange textRangeDateStartFact = paragraph6_1.AppendText(report.DateStartFact.ToShortDateString());
            TextRange textRangeDateEndFact = paragraph6_2.AppendText(report.DateEndFact.ToShortDateString());
            FormatText(textRangeDateStartFact);
            FormatText(textRangeDateEndFact);

            Table table7 = section.Tables[6] as Table;//Время монтажа/первого использования/погрузки на транспортное средство оборудования  за период бронирования
            TableRow tableRow7 = table7.Rows[0];
            Paragraph paragraph7 = tableRow7.Cells[0].Paragraphs[0];
            paragraph7.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            TextRange textRangeDateFirstUseEquipment = paragraph7.AppendText(report.DateFirstUseEquipment.ToShortDateString());
            FormatText(textRangeDateFirstUseEquipment);

            Table table8 = section.Tables[7] as Table;//Место использования оборудования
            TableRow tableRow8_1 = table8.Rows[0];
            Paragraph paragraph8_1 = tableRow8_1.Cells[1].Paragraphs[0];
            TextRange textRangePlaceOfUse = paragraph8_1.AppendText(report.IdPlaceOfUseNavigation.Title);//Место

            TableRow tableRow8_2 = table8.Rows[1];
            Paragraph paragraph8_2 = tableRow8_2.Cells[0].Paragraphs[0];
            TextRange textRangeDescriptionPlaceOfUse = paragraph8_2.AppendText(report.DescriptionPlaceOfUse);//Описание

            TableRow tableRow8_3 = table8.Rows[2];
            Paragraph paragraph8_3 = tableRow8_3.Cells[1].Paragraphs[0];
            TextRange textRangeNumberDocument = paragraph8_3.AppendText(report.NumberDocument);//Номер документа
            FormatText(textRangePlaceOfUse);
            FormatText(textRangeDescriptionPlaceOfUse);
            FormatText(textRangeNumberDocument);

            Table table9 = section.Tables[8] as Table;//Тип работы оборудования
            TableRow tableRow9 = table9.Rows[0];
            Paragraph paragraph9 = tableRow9.Cells[0].Paragraphs[0];
            TextRange textRangeTypeOfWork = paragraph9.AppendText(report.IdTypeOfWorkNavigation.Title);
            FormatText(textRangeTypeOfWork);

            Table table10 = section.Tables[9] as Table;//Количество выполненных измерений/отбора проб
            TableRow tableRow10 = table10.Rows[0];
            Paragraph paragraph10 = tableRow10.Cells[0].Paragraphs[0];
            TextRange textRangeNumberOfMeasurements = paragraph10.AppendText(report.NumberOfMeasurements);
            FormatText(textRangeNumberOfMeasurements);

            Table table11 = section.Tables[10] as Table;//Характеристика работы оборудования в период эксплуатации
            TableRow tableRow11 = table11.Rows[0];
            Paragraph paragraph11 = tableRow11.Cells[0].Paragraphs[0];
            TextRange textRangeCharacteristicsWork = paragraph11.AppendText(report.CharacteristicsWork);
            FormatText(textRangeCharacteristicsWork);

            Table table12 = section.Tables[11] as Table;//Выполнение ремонта оборудование за время бронирования
            TableRow tableRow12 = table12.Rows[2];
            Paragraph paragraph12_1 = tableRow12.Cells[0].Paragraphs[0];//дата начала
            Paragraph paragraph12_2 = tableRow12.Cells[1].Paragraphs[0];//дата окончания
            Paragraph paragraph12_3 = tableRow12.Cells[2].Paragraphs[0];//описание
            paragraph12_1.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            paragraph12_2.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            if (report.Repairs.Count != 0)
            {
                TextRange textRangeDateStartDowntime = paragraph12_1.AppendText(Repair.DateStartDowntime.ToShortDateString());
                TextRange textRangeDateEndDowntime = paragraph12_2.AppendText(Repair.DateEndDowntime.ToShortDateString());
                FormatText(textRangeDateStartDowntime);
                FormatText(textRangeDateEndDowntime);
            }
            if(report.ReplacementOfConsumables.Count != 0)
            {
                TextRange textRangeDiscriptionReplacementOfConsumable = paragraph12_3.AppendText(ReplacementOfConsumable.Description);
                FormatText(textRangeDiscriptionReplacementOfConsumable);
            }

            Table table13 = section.Tables[12] as Table;//Дата демонтажа/последнего использования оборудования за период бронирования
            TableRow tableRow13 = table13.Rows[0];
            Paragraph paragraph13 = tableRow13.Cells[0].Paragraphs[0];
            paragraph13.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            TextRange textRangeDateLastUseEquipment = paragraph13.AppendText(report.DateLastUseEquipment.ToShortDateString());
            FormatText(textRangeDateLastUseEquipment);

            Table table14 = section.Tables[13] as Table;//Дата подписания отчета ответственным исполнителем
            TableRow tableRow14 = table14.Rows[0];
            Paragraph paragraph14 = tableRow14.Cells[0].Paragraphs[0];
            paragraph14.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            TextRange textRangeDateSigningReportScientists = paragraph14.AppendText(report.DateSigningReportScientists.ToShortDateString());
            FormatText(textRangeDateSigningReportScientists);

            Table table15 = section.Tables[14] as Table;//Дата возврата оборудования  владельцу
            TableRow tableRow15 = table15.Rows[0];
            Paragraph paragraph15 = tableRow15.Cells[0].Paragraphs[0];
            paragraph15.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            TextRange textRangeDateReturn = paragraph15.AppendText(report.DateReturn.Value.ToShortDateString());
            FormatText(textRangeDateReturn);

            Table table16 = section.Tables[15] as Table;//состояние оборудования и фотки
            TableRow tableRow16_1 = table16.Rows[0];
            Paragraph paragraph16 = tableRow16_1.Cells[0].Paragraphs[0];
            TextRange textRangeStatusEquipment = paragraph16.AppendText(report.StatusEquipment);
            FormatText(textRangeStatusEquipment);
            TableRow tableRow16_2 = table16.Rows[1];
            DocPicture docPicture2;
            for (int i = 0; i < ImagesResponsable.Count; i++)
            {
                Paragraph p = tableRow2_2.Cells[i].Paragraphs[0];
                docPicture2 = p.AppendPicture(Images[i]);
                docPicture2.Width = 150;
                docPicture2.Height = 150;
            }

            Table table17 = section.Tables[16] as Table;
            TableRow tableRow17_1 = table17.Rows[1];
            TableRow tableRow17_2 = table17.Rows[2];
            Paragraph paragraph17_1 = tableRow17_1.Cells[0].Paragraphs[0];//притензия есть/нет у владельца
            Paragraph paragraph17_2 = tableRow17_1.Cells[1].Paragraphs[0];//притензия есть/нет у исполнителя
            Paragraph paragraph17_3 = tableRow17_2.Cells[0].Paragraphs[0];//описание владельца
            Paragraph paragraph17_4 = tableRow17_2.Cells[1].Paragraphs[0];//описание исполнителя
            TextRange textRangeConflictSituationScientists;
            TextRange textRangeDiscriptionConflictSituationScientists;
            TextRange textRangeConflictSituationResponsible;
            TextRange textRangeDiscriptionConflictSituationResponsible;
            if (report.ConflictSituationResponsible != null)
            {
                textRangeConflictSituationResponsible = paragraph17_1.AppendText("Есть замечание");
                textRangeDiscriptionConflictSituationResponsible = paragraph17_3.AppendText(report.ConflictSituationResponsible);
                FormatText(textRangeDiscriptionConflictSituationResponsible);
            }
            else
                textRangeConflictSituationResponsible = paragraph17_1.AppendText("Претензий не имею");

            if(report.ConflictSituationScientists != null)
            {
                textRangeConflictSituationScientists = paragraph17_2.AppendText("Есть замечание");
                textRangeDiscriptionConflictSituationScientists = paragraph17_4.AppendText(report.ConflictSituationScientists);
                FormatText(textRangeDiscriptionConflictSituationScientists);
            }
            else
                textRangeConflictSituationScientists = paragraph17_2.AppendText("Претензий не имею");
            FormatText(textRangeConflictSituationScientists);
            FormatText(textRangeConflictSituationResponsible);

            Table table18 = section.Tables[17] as Table;
            TableRow tableRow18 = table18.Rows[0];
            Paragraph paragraph18 = tableRow18.Cells[0].Paragraphs[0];
            paragraph18.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            TextRange textRangeDateSigningReportReponsibleScientists = paragraph18.AppendText(report.DateSigningReportReponsibleScientists.Value.ToShortDateString());
            FormatText(textRangeDateSigningReportReponsibleScientists);

            try
            {
                DateTime dateTime = DateTime.Now;
                string file = $"Отчёт об использовании научного оборудования - {report.IdBookingNavigation.IdEquipmentNavigation.Name}(дата подписания ответственным сотрудником -{report.DateSigningReportReponsibleScientists.Value.ToShortDateString()}).docx";
                document.SaveToFile(file, FileFormat.Docx);
                ProcessStartInfo process = new ProcessStartInfo();
                process.FileName = "explorer.exe";
                process.Arguments = file;
                process.UseShellExecute = true;
                System.Diagnostics.Process.Start(process);

            }
            catch 
            {
                MessageBox.Show("");
                return;
            }
        }
        public static void FormatText(TextRange textRange)
        {
            textRange.CharacterFormat.FontName = "Times New Roman";
            textRange.CharacterFormat.FontSize = 12;
        }
        
    }
}
