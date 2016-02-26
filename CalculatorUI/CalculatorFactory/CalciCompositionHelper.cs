using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using CalculatorContract;
using System.Diagnostics;

namespace CompositionHelper
{
    public class CalciCompositionHelper
    {
        [ImportMany]
        public System.Lazy<ICalculator, IDictionary<string, object>>[] CalciPlugins { get; set; }

        /// <summary>
        /// Assembles the calculator components
        /// </summary>
        public void AssembleCalculatorComponents()
        {
            try
            {
                //Creating an instance of aggregate catalog. It aggregates other catalogs
                var aggregateCatalog = new AggregateCatalog();
                
                //Build the directory path where the parts will be available
                var directoryPath =
                    string.Concat(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                .Split('\\').Reverse().Skip(3).Reverse().Aggregate((a, b) => a + "\\" + b)
                                , "\\", "ExportComponents\\Components");


                Debug.WriteLine("Path is: [" + directoryPath + "]");

                //Load parts from the available dlls in the specified path using the directory catalog
                var directoryCatalog = new DirectoryCatalog(directoryPath, "*.dll");

                //Load parts from the current assembly if available
                var asmCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

                //Add to the aggregate catalog
                aggregateCatalog.Catalogs.Add(directoryCatalog);
                aggregateCatalog.Catalogs.Add(asmCatalog);

                //Crete the composition container
                var container = new CompositionContainer(aggregateCatalog);

                // Composable parts are created here i.e. the Import and Export components assembles here
                container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sends the result back to the client
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public int GetResult(int num1, int num2, string operationType)
        {
            int result = 0;
            foreach (var CalciPlugin in CalciPlugins)
            {
                string s = (string) CalciPlugin.Metadata["CalciMetaData"];

                Debug.WriteLine("This discovered status: " + s);

                if ((string)CalciPlugin.Metadata["CalciMetaData"] == operationType)
                {
                    result = CalciPlugin.Value.GetNumber(num1, num2);
                    break;
                }                               
            }
            return result;           
        }
    }
}
