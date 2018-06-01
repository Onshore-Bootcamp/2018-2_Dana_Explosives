using Business_Layer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer
{
    public class CalcBAO
    {
        //Counting munitionIDs in videos table
        public List<MunitionBO> CountVideos(DataTable table)
        {
            //Instantiations
            List<MunitionBO> munitionBO = new List<MunitionBO>();
            List<Int64> munitionID = new List<Int64>();

            //Adding all munitionIDs to list
            foreach (DataRow row in table.Rows)
            {
                munitionID.Add((Int64)row["MunitionID"]);
            }

            //Keeping one instance of each munitionID
            munitionID = munitionID.Distinct().ToList();

            //Using LINQ to select all rows from table where munitionID = id
            foreach (Int64 id in munitionID)
            {
                var count = from row in table.Rows.Cast<DataRow>()
                            where row.Field<Int64>("MunitionID") == id
                            select row;

                //Instantiate new MunitionBO and adding current munitionID to object
                MunitionBO munitions = new MunitionBO();
                munitions.MunitionID = id;

                //Count how many times that id was in the table
                munitions.VideoCount = count.Count<DataRow>();

                //Adding object to list for controller
                munitionBO.Add(munitions);
            }
            //Returning list
            return munitionBO;
        }
    }
}
