using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPILab_2_4_CountDuctsOnLevels

    //Пример ElementLrvelFilter /*https://www.revitapidocs.com/2015/844e4928-e11a-563f-b1e4-d4d16b8bd76b.htm*/
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            #region Определение ID уровня. Оставлено на память.
            List<Level> levels = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .OfType<Level>()
                .ToList();

            List<ElementId> levels1Id = new List<ElementId>();
            foreach (Element level in levels)
                {if (level.Name=="Level 1")
                    levels1Id.Add(level.Id);
                    }

            List<ElementId> levels2Id = new List<ElementId>();
            foreach (Element level in levels)
            {
                if (level.Name == "Level 2")
                    levels2Id.Add(level.Id);
            }
            #endregion

            List<Element> allDuctsOnLevel1 = new FilteredElementCollector(doc)
                .OfClass(typeof(Duct))
                .Where(x => x.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM).AsValueString() == "Level 1")
                .ToList();

            List<Element> allDuctsOnLevel2 = new FilteredElementCollector(doc)
                .OfClass(typeof(Duct))
                .Where(x => x.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM).AsValueString() == "Level 2")
                .ToList();

            TaskDialog.Show("Количество воздуховодов поэтажно", $"Количество воздуховодов на 1 (ID: {levels1Id[0]}) этаже: {allDuctsOnLevel1.Count}\nКоличество воздуховодов на 2(ID:{levels2Id[0]}) этаже: {allDuctsOnLevel2.Count} ");
            return Result.Succeeded;

        }
    }
}
