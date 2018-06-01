using System.Collections.Generic;
using DataLayer.Models;
using Explosives.Models;

namespace Explosives.Mapping
{
    public static class MunitionsMapper
    {

        //Mapping one Munition from DO to PO
        public static MunitionsPO MapDOtoPO(MunitionsDO from)
        {
            MunitionsPO to = new MunitionsPO();
            to.MunitionID = from.MunitionID;
            to.Munition = from.Munition;
            to.Description = from.Description;
            to.TopicID = from.TopicID;

            //Returning Munition
            return to;
        }

        //Mapping list of Munitions from DO to PO
        public static List<MunitionsPO> MapDOtoPO(List<MunitionsDO> from)
        {
            List<MunitionsPO> to = new List<MunitionsPO>();

            if (from != null)
            {
                foreach (MunitionsDO item in from)
                {
                    //Changing a DO munition to a PO munition
                    MunitionsPO mappedItem = MapDOtoPO(item);
                    //Adding PO munition to a list of PO munitions
                    to.Add(mappedItem);
                }
            }

            //Returning list of Munitions
            return to;
        }

        //Mapping one Munition from PO to DO
        public static MunitionsDO MapPOtoDO(MunitionsPO from)
        {
            MunitionsDO to = new MunitionsDO();
            to.MunitionID = from.MunitionID;
            to.Munition = from.Munition;
            to.Description = from.Description;
            to.TopicID = from.TopicID;

            //Returning Munition
            return to;
        }

        //Mapping list of Munitions from PO to DO
        public static List<MunitionsDO> MapPOtoDO(List<MunitionsPO> from)
        {
            List<MunitionsDO> to = new List<MunitionsDO>();

            if (from != null)
            {
                foreach (MunitionsPO munition in from)
                {
                    //Changing a PO munition to a DO munition
                    MunitionsDO mappedMunition = MapPOtoDO(munition);
                    //Adding DO munition to a list of DO munitions
                    to.Add(mappedMunition);
                }
            }
            //Returning list of Munitions
            return to;
        }

        //Mapping one Topic from DO to PO
        public static TopicPO MapDOtoPO(TopicDO from)
        {
            TopicPO to = new TopicPO();
            to.TopicID = from.TopicID;
            to.Topic = from.Topic;

            //Returning Topic
            return to;
        }

        //Mapping list of Topics from PO to DO
        public static List<TopicPO> MapDOtoPO(List<TopicDO> from)
        {
            List<TopicPO> to = new List<TopicPO>();

            if (from != null)
            {
                foreach (TopicDO item in from)
                {
                    //Changing a DO munition to a PO munition
                    TopicPO mappedItem = MapDOtoPO(item);
                    //Adding PO munition to a list of PO munitions
                    to.Add(mappedItem);
                }
            }
            //Returning list of Topics
            return to;
        }
    }

}