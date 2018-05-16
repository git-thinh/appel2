using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using ProtoBuf;
using System.Net;
using HtmlAgilityPack;
using System.Net.Sockets;
using System.Web;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using NAudio.Wave;
using YoutubeExplode.Models.MediaStreams;
using YoutubeExplode.Internal;
//using Fizzler.Systems.HtmlAgilityPack;

namespace appel
{ 
    public class api_mp3 : api_base, IAPI
    {
        public void Close() { }

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            string path_file = string.Empty;

            switch (m.KEY) {
                case _API.MP3_PLAY:
                    path_file = (string)m.Input;

                    using (var ms = File.OpenRead(path_file))
                    using (var rdr = new Mp3FileReader(ms))
                    using (var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr))
                    using (var baStream = new BlockAlignReductionStream(wavStream))
                    using (var waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(baStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(100);
                        }
                    }
                    break;
            }

            return m;
        }
    }
}
