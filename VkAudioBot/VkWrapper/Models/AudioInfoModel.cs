using System;
using System.Collections.Generic;
using System.Text;

namespace VkAudioBot.VkWrapper.Models
{
    public class AudioInfoModel
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public bool Allow { get; set; }
        public int NumberInPlayList { get; set; }

        public AudioInfoModel()
        {
            
        }

        public AudioInfoModel(string title, string artist, bool allow, int numberInPlayList)
        {
            Title = title;
            Artist = artist;
            Allow = allow;
            NumberInPlayList = numberInPlayList;
        }
    }
}
