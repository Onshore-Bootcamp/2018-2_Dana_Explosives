using DataLayer.Models;
using Explosives.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explosives.Mapping
{
    public class VideosMapper
    {
        public static VideosPO MapDOtoPO(VideosDO from)
        {
            VideosPO to = new VideosPO();
            to.VideoID = from.VideoID;
            to.VideoPath = from.VideoPath;
            to.VideoName = from.VideoName;
            to.VideoDescription = from.VideoDescription;
            to.MunitionID = from.MunitionID;
            to.UserID = from.UserID;

            return to;
        }

        public static List<VideosPO> MapDOtoPO(List<VideosDO> from)
        {
            List<VideosPO> to = new List<VideosPO>();

            if (from != null)
            {
                foreach (VideosDO item in from)
                {
                    VideosPO mappedItem = MapDOtoPO(item);
                    to.Add(mappedItem);
                }
            }
            return to;
        }

        public static VideosDO MapPOtoDO(VideosPO from)
        {
            VideosDO to = new VideosDO();
            to.VideoID = from.VideoID;
            to.VideoName = from.VideoName;
            to.VideoPath = from.VideoPath;
            to.VideoDescription = from.VideoDescription;
            to.MunitionID = from.MunitionID;
            to.UserID = from.UserID;

            return to;
        }

        public static List<VideosDO> MapPOtoDO(List<VideosPO> from)
        {
            List<VideosDO> to = new List<VideosDO>();

            if (from != null)
            {
                foreach (VideosPO item in from)
                {
                    VideosDO mappedItem = MapPOtoDO(item);
                    to.Add(mappedItem);
                }
            }
            return to;
        }
    }
}