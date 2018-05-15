using System;
using System.Collections.Generic;
using System.Text;

namespace appel
{

    /// <summary>
    /// Information about a YouTube channel.
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// ID of this channel.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Title of this channel.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Logo image URL of this channel.
        /// </summary>
        public string LogoUrl { get; }

        /// <summary />
        public Channel(string id, string title, string logoUrl)
        {
            Id = id; //.GuardNotNull(nameof(id));
            Title = title; //.GuardNotNull(nameof(title));
            LogoUrl = logoUrl; //.GuardNotNull(nameof(logoUrl));
        }

        /// <inheritdoc />
        public override string ToString() => Title;
    }

    internal class PlayerContext
    {
        public string SourceUrl { get; }

        public string Sts { get; }

        public PlayerContext(string sourceUrl, string sts)
        {
            SourceUrl = sourceUrl;
            Sts = sts;
        }
    }

    /// <summary>
    /// Set of thumbnails for a video.
    /// </summary>
    public class ThumbnailSet
    {
        private readonly string _videoId;

        /// <summary>
        /// Low resolution thumbnail URL.
        /// </summary>
        public string LowResUrl => $"https://img.youtube.com/vi/{_videoId}/default.jpg";

        /// <summary>
        /// Medium resolution thumbnail URL.
        /// </summary>
        public string MediumResUrl => $"https://img.youtube.com/vi/{_videoId}/mqdefault.jpg";

        /// <summary>
        /// High resolution thumbnail URL.
        /// </summary>
        public string HighResUrl => $"https://img.youtube.com/vi/{_videoId}/hqdefault.jpg";

        /// <summary>
        /// Standard resolution thumbnail URL.
        /// Not always available.
        /// </summary>
        public string StandardResUrl => $"https://img.youtube.com/vi/{_videoId}/sddefault.jpg";

        /// <summary>
        /// Max resolution thumbnail URL.
        /// Not always available.
        /// </summary>
        public string MaxResUrl => $"https://img.youtube.com/vi/{_videoId}/maxresdefault.jpg";

        /// <summary />
        public ThumbnailSet(string videoId)
        {
            _videoId = videoId;
        }
    }

    /// <summary>
    /// User activity statistics.
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// View count.
        /// </summary>
        public long ViewCount { get; }

        /// <summary>
        /// Like count.
        /// </summary>
        public long LikeCount { get; }

        /// <summary>
        /// Dislike count.
        /// </summary>
        public long DislikeCount { get; }

        /// <summary>
        /// Average user rating in stars (1 star to 5 stars).
        /// </summary>
        public double AverageRating
        {
            get
            {
                if (LikeCount + DislikeCount == 0) return 0;
                return 1 + 4.0 * LikeCount / (LikeCount + DislikeCount);
            }
        }

        /// <summary />
        public Statistics(long viewCount, long likeCount, long dislikeCount)
        {
            ViewCount = viewCount;
            LikeCount = likeCount;
            DislikeCount = dislikeCount;
        }
    }

    /// <summary>
    /// Information about a YouTube video.
    /// </summary>
    public class oVideo
    {
        /// <summary>
        /// ID of this video.
        /// </summary> 
        public string Id { get; }

        /// <summary>
        /// Author of this video.
        /// </summary> 
        public string Author { get; }

        /// <summary>
        /// Upload date of this video.
        /// </summary>
        public DateTimeOffset UploadDate { get; }

        /// <summary>
        /// Title of this video.
        /// </summary> 
        public string Title { get; }

        /// <summary>
        /// Description of this video.
        /// </summary> 
        public string Description { get; }

        /// <summary>
        /// Thumbnails of this video.
        /// </summary> 
        public ThumbnailSet Thumbnails { get; }

        /// <summary>
        /// Duration of this video.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Search keywords of this video.
        /// </summary> 
        public string[] Keywords { get; }

        /// <summary>
        /// Statistics of this video.
        /// </summary> 
        public Statistics Statistics { get; }

        /// <summary />
        public oVideo(string id, string author, DateTimeOffset uploadDate, string title, string description,
            ThumbnailSet thumbnails, TimeSpan duration, string[] keywords, Statistics statistics)
        {
            Id = id;
            Author = author;
            UploadDate = uploadDate;
            Title = title;
            Description = description;
            Thumbnails = thumbnails;
            Duration = duration;
            Keywords = keywords;
            Statistics = statistics;
        }

        /// <inheritdoc />
        public override string ToString() => Title;
    }
    
    public class VideoUnavailableException : Exception
    {
        /// <summary>
        /// ID of the video.
        /// </summary>
        public string VideoId { get; }

        /// <summary>
        /// Error code reported by YouTube.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// Error reason reported by YouTube.
        /// </summary>
        public string Reason { get; }

        /// <inheritdoc />
        public override string Message => $"Video [{VideoId}] is not available and cannot be processed." +
                                          Environment.NewLine +
                                          $"Error code: {Code}" +
                                          Environment.NewLine +
                                          $"Error reason: {Reason}";

        /// <summary />
        public VideoUnavailableException(string videoId, int code, string reason)
        {
            VideoId = videoId;
            Code = code;
            Reason = reason;
        }
    }
    
    /// <summary>
    /// Set of all available media stream infos.
    /// </summary>
    public class MediaStreamInfoSet
    {
        /// <summary>
        /// Muxed streams.
        /// </summary>
        public List<MuxedStreamInfo> Muxed { get; }

        /// <summary>
        /// Audio-only streams.
        /// </summary>
        public List<AudioStreamInfo> Audio { get; }

        /// <summary>
        /// Video-only streams.
        /// </summary>
        public List<VideoStreamInfo> Video { get; }

        /// <summary>
        /// Raw HTTP Live Streaming (HLS) URL to the m3u8 playlist.
        /// Null if not a live stream.
        /// </summary>
        public string HlsLiveStreamUrl { get; }

        /// <summary />
        public MediaStreamInfoSet(List<MuxedStreamInfo> muxed,
            List<AudioStreamInfo> audio,
            List<VideoStreamInfo> video,
            string hlsLiveStreamUrl)
        {
            Muxed = muxed; //;//.GuardNotNull(nameof(muxed));
            Audio = audio; //;//.GuardNotNull(nameof(audio));
            Video = video; //;//.GuardNotNull(nameof(video));
            HlsLiveStreamUrl = hlsLiveStreamUrl;
        }
    }

    /// <summary>
    /// Metadata associated with a certain <see cref="MediaStream"/> that contains both audio and video.
    /// </summary>
    public class MuxedStreamInfo : MediaStreamInfo
    {
        /// <summary>
        /// Audio encoding of the associated stream.
        /// </summary>
        public AudioEncoding AudioEncoding { get; }

        /// <summary>
        /// Video encoding of the associated stream.
        /// </summary>
        public VideoEncoding VideoEncoding { get; }

        /// <summary>
        /// Video quality of the associated stream.
        /// </summary>
        public VideoQuality VideoQuality { get; }

        /// <summary>
        /// Video quality label of the associated stream.
        /// </summary>
        public string VideoQualityLabel { get; }

        /// <summary />
        public MuxedStreamInfo(int itag, string url, long size) : base(itag, url, size)
        {
            AudioEncoding = GetAudioEncoding(itag);
            VideoEncoding = GetVideoEncoding(itag);
            VideoQuality = GetVideoQuality(itag);
            VideoQualityLabel = VideoQuality.GetVideoQualityLabel();
        }
    }

    /// <summary>
    /// Metadata associated with a certain <see cref="MediaStream"/> that contains only audio.
    /// </summary>
    public class AudioStreamInfo : MediaStreamInfo
    {
        /// <summary>
        /// Bitrate (bit/s) of the associated stream.
        /// </summary>
        public long Bitrate { get; }

        /// <summary>
        /// Audio encoding of the associated stream.
        /// </summary>
        public AudioEncoding AudioEncoding { get; }

        /// <summary />
        public AudioStreamInfo(int itag, string url, long size, long bitrate)
            : base(itag, url, size)
        {
            Bitrate = bitrate;//.GuardNotNegative(nameof(bitrate));
            AudioEncoding = GetAudioEncoding(itag);
        }
    }

    /// <summary>
    /// Metadata associated with a certain <see cref="MediaStream"/> that contains only video.
    /// </summary>
    public class VideoStreamInfo : MediaStreamInfo
    {
        /// <summary>
        /// Video bitrate (bits/s) of the associated stream.
        /// </summary>
        public long Bitrate { get; }

        /// <summary>
        /// Video encoding of the associated stream.
        /// </summary>
        public VideoEncoding VideoEncoding { get; }

        /// <summary>
        /// Video quality of the associated stream.
        /// </summary>
        public VideoQuality VideoQuality { get; }

        /// <summary>
        /// Video resolution of the associated stream.
        /// </summary>
        public VideoResolution Resolution { get; }

        /// <summary>
        /// Video framerate (FPS) of the associated stream.
        /// </summary>
        public int Framerate { get; }

        /// <summary>
        /// Video quality label of the associated stream.
        /// </summary>
        public string VideoQualityLabel { get; }

        /// <summary />
        public VideoStreamInfo(int itag, string url, long size, long bitrate, VideoResolution resolution, int framerate)
            : base(itag, url, size)
        {
            Bitrate = bitrate;//.GuardNotNegative(nameof(bitrate));
            VideoEncoding = GetVideoEncoding(itag);
            VideoQuality = GetVideoQuality(itag);
            Resolution = resolution;
            Framerate = framerate;//.GuardNotNegative(nameof(framerate));
            VideoQualityLabel = VideoQuality.GetVideoQualityLabel(framerate);
        }

        /// <summary />
        public VideoStreamInfo(int itag, string url, long size, long bitrate, VideoResolution resolution, int framerate,
            string videoQualityLabel)
            : base(itag, url, size)
        {
            Bitrate = bitrate;//.GuardNotNegative(nameof(bitrate));
            VideoEncoding = GetVideoEncoding(itag);
            Resolution = resolution;
            Framerate = framerate;//.GuardNotNegative(nameof(framerate));
            VideoQualityLabel = videoQualityLabel;//.GuardNotNull(nameof(videoQualityLabel));
            VideoQuality = GetVideoQuality(videoQualityLabel);
        }
    }

    /// <summary>
    /// Media stream container type.
    /// </summary>
    public enum Container
    {
        /// <summary>
        /// MPEG-4 Part 14 (.mp4).
        /// </summary>
        Mp4,

        /// <summary>
        /// MPEG-4 Part 14 audio-only (.m4a).
        /// </summary>
        M4A,

        /// <summary>
        /// Web Media (.webm).
        /// </summary>
        WebM,

        /// <summary>
        /// 3rd Generation Partnership Project (.3gpp).
        /// </summary>
        Tgpp,

        /// <summary>
        /// Flash Video (.flv).
        /// </summary>
        Flv
    }

    /// <summary>
    /// Audio encoding.
    /// </summary>
    public enum AudioEncoding
    {
        /// <summary>
        /// MPEG-2 Audio Layer III.
        /// </summary>
        Mp3,

        /// <summary>
        /// Advanced Audio Coding.
        /// </summary>
        Aac,

        /// <summary>
        /// Vorbis.
        /// </summary>
        Vorbis,

        /// <summary>
        /// Opus.
        /// </summary>
        Opus
    }

    /// <summary>
    /// Video quality.
    /// </summary>
    public enum VideoQuality
    {
        /// <summary>
        /// Low quality (144p).
        /// </summary>
        Low144,

        /// <summary>
        /// Low quality (240p).
        /// </summary>
        Low240,

        /// <summary>
        /// Medium quality (360p).
        /// </summary>
        Medium360,

        /// <summary>
        /// Medium quality (480p).
        /// </summary>
        Medium480,

        /// <summary>
        /// High quality (720p).
        /// </summary>
        High720,

        /// <summary>
        /// High quality (1080p).
        /// </summary>
        High1080,

        /// <summary>
        /// High quality (1440p).
        /// </summary>
        High1440,

        /// <summary>
        /// High quality (2160p).
        /// </summary>
        High2160,

        /// <summary>
        /// High quality (2880p).
        /// </summary>
        High2880,

        /// <summary>
        /// High quality (3072p).
        /// </summary>
        High3072,

        /// <summary>
        /// High quality (4320p).
        /// </summary>
        High4320
    }

    /// <summary>
    /// Video encoding.
    /// </summary>
    public enum VideoEncoding
    {
        /// <summary>
        /// MPEG-4 Visual.
        /// </summary>
        Mp4V,

        /// <summary>
        /// MPEG-4 Part 10, Advanced Video Coding.
        /// </summary>
        H263,

        /// <summary>
        /// MPEG-4 Part 10, Advanced Video Coding.
        /// </summary>
        H264,

        /// <summary>
        /// VP8.
        /// </summary>
        Vp8,

        /// <summary>
        /// VP9.
        /// </summary>
        Vp9
    }
    
    /// <summary>
    /// Width and height of a video.
    /// </summary>
    public partial struct VideoResolution : IEquatable<VideoResolution>
    {
        /// <summary>
        /// Viewport width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Viewport height.
        /// </summary>
        public int Height { get; }

        /// <summary />
        public VideoResolution(int width, int height)
        {
            Width = width;//.GuardNotNegative(nameof(width));
            Height = height;//.GuardNotNegative(nameof(height));
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is VideoResolution)
            {
                VideoResolution other = (VideoResolution)obj;
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc />
        public bool Equals(VideoResolution other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }

        /// <inheritdoc />
        public override string ToString() => $"{Width}x{Height}";
    }

    public partial struct VideoResolution
    {
        /// <summary />
        public static bool operator ==(VideoResolution r1, VideoResolution r2) => r1.Equals(r2);

        /// <summary />
        public static bool operator !=(VideoResolution r1, VideoResolution r2) => !(r1 == r2);
    }
    
    internal class ItagDescriptor
    {
        public Container Container { get; }

        public AudioEncoding? AudioEncoding { get; }

        public VideoEncoding? VideoEncoding { get; }

        public VideoQuality? VideoQuality { get; }

        public ItagDescriptor(Container container,
            AudioEncoding? audioEncoding,
            VideoEncoding? videoEncoding,
            VideoQuality? videoQuality)
        {
            Container = container;
            AudioEncoding = audioEncoding;
            VideoEncoding = videoEncoding;
            VideoQuality = videoQuality;
        }
    }

    /// <summary>
    /// Metadata associated with a certain <see cref="MediaStream"/>.
    /// </summary>
    public abstract partial class MediaStreamInfo
    {
        /// <summary>
        /// Unique tag that identifies the properties of the associated stream.
        /// </summary>
        public int Itag { get; }

        /// <summary>
        /// URL of the endpoint that serves the associated stream.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Container type of the associated stream.
        /// </summary>
        public Container Container { get; }

        /// <summary>
        /// Content length (bytes) of the associated stream.
        /// </summary>
        public long Size { get; }

        /// <summary />
        protected MediaStreamInfo(int itag, string url, long size)
        {
            Itag = itag;
            Url = url;//.GuardNotNull(nameof(url));
            Container = GetContainer(itag);
            Size = size;//.GuardNotNegative(nameof(size));
        }

        /// <inheritdoc />
        public override string ToString() => $"{Itag} ({Container})";
    }

    public abstract partial class MediaStreamInfo
    {
        private static readonly Dictionary<int, ItagDescriptor> ItagMap = new Dictionary<int, ItagDescriptor>
        {
            // Muxed
            {5, new ItagDescriptor(Container.Flv, AudioEncoding.Mp3, VideoEncoding.H263, VideoQuality.Low144)},
            {6, new ItagDescriptor(Container.Flv, AudioEncoding.Mp3, VideoEncoding.H263, VideoQuality.Low240)},
            {13, new ItagDescriptor(Container.Tgpp, AudioEncoding.Aac, VideoEncoding.Mp4V, VideoQuality.Low144)},
            {17, new ItagDescriptor(Container.Tgpp, AudioEncoding.Aac, VideoEncoding.Mp4V, VideoQuality.Low144)},
            {18, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Medium360)},
            {22, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.High720)},
            {34, new ItagDescriptor(Container.Flv, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Medium360)},
            {35, new ItagDescriptor(Container.Flv, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Medium480)},
            {36, new ItagDescriptor(Container.Tgpp, AudioEncoding.Aac, VideoEncoding.Mp4V, VideoQuality.Low240)},
            {37, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.High1080)},
            {38, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.High3072)},
            {43, new ItagDescriptor(Container.WebM, AudioEncoding.Vorbis, VideoEncoding.Vp8, VideoQuality.Medium360)},
            {44, new ItagDescriptor(Container.WebM, AudioEncoding.Vorbis, VideoEncoding.Vp8, VideoQuality.Medium480)},
            {45, new ItagDescriptor(Container.WebM, AudioEncoding.Vorbis, VideoEncoding.Vp8, VideoQuality.High720)},
            {46, new ItagDescriptor(Container.WebM, AudioEncoding.Vorbis, VideoEncoding.Vp8, VideoQuality.High1080)},
            {59, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Medium480)},
            {78, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Medium480)},
            {82, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Medium360)},
            {83, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Medium480)},
            {84, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.High720)},
            {85, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.High1080)},
            {91, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Low144)},
            {92, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Low240)},
            {93, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Medium360)},
            {94, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Medium480)},
            {95, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.High720)},
            {96, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.High1080)},
            {100, new ItagDescriptor(Container.WebM, AudioEncoding.Vorbis, VideoEncoding.Vp8, VideoQuality.Medium360)},
            {101, new ItagDescriptor(Container.WebM, AudioEncoding.Vorbis, VideoEncoding.Vp8, VideoQuality.Medium480)},
            {102, new ItagDescriptor(Container.WebM, AudioEncoding.Vorbis, VideoEncoding.Vp8, VideoQuality.High720)},
            {132, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Low240)},
            {151, new ItagDescriptor(Container.Mp4, AudioEncoding.Aac, VideoEncoding.H264, VideoQuality.Low144)},

            // Video-only (mp4)
            {133, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.Low240)},
            {134, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.Medium360)},
            {135, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.Medium480)},
            {136, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High720)},
            {137, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High1080)},
            {138, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High4320)},
            {160, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.Low144)},
            {212, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.Medium480)},
            {213, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.Medium480)},
            {214, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High720)},
            {215, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High720)},
            {216, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High1080)},
            {217, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High1080)},
            {264, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High1440)},
            {266, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High2160)},
            {298, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High720)},
            {299, new ItagDescriptor(Container.Mp4, null, VideoEncoding.H264, VideoQuality.High1080)},

            // Video-only (webm)
            {167, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp8, VideoQuality.Medium360)},
            {168, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp8, VideoQuality.Medium480)},
            {169, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp8, VideoQuality.High720)},
            {170, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp8, VideoQuality.High1080)},
            {218, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp8, VideoQuality.Medium480)},
            {219, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp8, VideoQuality.Medium480)},
            {242, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Low240)},
            {243, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Medium360)},
            {244, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Medium480)},
            {245, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Medium480)},
            {246, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Medium480)},
            {247, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High720)},
            {248, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High1080)},
            {271, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High1440)},
            {272, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High2160)},
            {278, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Low144)},
            {302, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High720)},
            {303, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High1080)},
            {308, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High1440)},
            {313, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High2160)},
            {315, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High2160)},
            {330, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Low144)},
            {331, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Low240)},
            {332, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Medium360)},
            {333, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.Medium480)},
            {334, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High720)},
            {335, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High1080)},
            {336, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High1440)},
            {337, new ItagDescriptor(Container.WebM, null, VideoEncoding.Vp9, VideoQuality.High2160)},

            // Audio-only (mp4)
            {139, new ItagDescriptor(Container.M4A, AudioEncoding.Aac, null, null)},
            {140, new ItagDescriptor(Container.M4A, AudioEncoding.Aac, null, null)},
            {141, new ItagDescriptor(Container.M4A, AudioEncoding.Aac, null, null)},
            {256, new ItagDescriptor(Container.M4A, AudioEncoding.Aac, null, null)},
            {258, new ItagDescriptor(Container.M4A, AudioEncoding.Aac, null, null)},
            {325, new ItagDescriptor(Container.M4A, AudioEncoding.Aac, null, null)},
            {328, new ItagDescriptor(Container.M4A, AudioEncoding.Aac, null, null)},

            // Audio-only (webm)
            {171, new ItagDescriptor(Container.WebM, AudioEncoding.Vorbis, null, null)},
            {172, new ItagDescriptor(Container.WebM, AudioEncoding.Vorbis, null, null)},
            {249, new ItagDescriptor(Container.WebM, AudioEncoding.Opus, null, null)},
            {250, new ItagDescriptor(Container.WebM, AudioEncoding.Opus, null, null)},
            {251, new ItagDescriptor(Container.WebM, AudioEncoding.Opus, null, null)}
        };

        /// <summary>
        /// Gets container type for the given itag.
        /// </summary>
        protected static Container GetContainer(int itag)
        {
            var result = ItagMap.GetOrDefault(itag)?.Container;

            if (!result.HasValue)
                throw new ArgumentOutOfRangeException(nameof(itag), $"Unexpected itag [{itag}].");

            return result.Value;
        }

        /// <summary>
        /// Gets audio encoding for the given itag.
        /// </summary>
        protected static AudioEncoding GetAudioEncoding(int itag)
        {
            var result = ItagMap.GetOrDefault(itag)?.AudioEncoding;

            if (!result.HasValue)
                throw new ArgumentOutOfRangeException(nameof(itag), $"Unexpected itag [{itag}].");

            return result.Value;
        }

        /// <summary>
        /// Gets video encoding for the given itag.
        /// </summary>
        protected static VideoEncoding GetVideoEncoding(int itag)
        {
            var result = ItagMap.GetOrDefault(itag)?.VideoEncoding;

            if (!result.HasValue)
                throw new ArgumentOutOfRangeException(nameof(itag), $"Unexpected itag [{itag}].");

            return result.Value;
        }

        /// <summary>
        /// Gets video quality for the given itag.
        /// </summary>
        protected static VideoQuality GetVideoQuality(int itag)
        {
            var result = ItagMap.GetOrDefault(itag)?.VideoQuality;

            if (!result.HasValue)
                throw new ArgumentOutOfRangeException(nameof(itag), $"Unexpected itag [{itag}].");

            return result.Value;
        }

        /// <summary>
        /// Gets video quality for the given quality label.
        /// </summary>
        protected static VideoQuality GetVideoQuality(string label)
        {
            if (label.StartsWith("144p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.Low144;

            if (label.StartsWith("240p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.Low240;

            if (label.StartsWith("360p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.Medium360;

            if (label.StartsWith("480p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.Medium480;

            if (label.StartsWith("720p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.High720;

            if (label.StartsWith("1080p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.High1080;

            if (label.StartsWith("1440p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.High1440;

            if (label.StartsWith("2160p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.High2160;

            if (label.StartsWith("2880p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.High2880;

            if (label.StartsWith("3072p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.High3072;

            if (label.StartsWith("4320p", StringComparison.OrdinalIgnoreCase))
                return VideoQuality.High4320;

            throw new ArgumentOutOfRangeException(nameof(label), $"Unexpected video quality label [{label}].");
        }

        /// <summary>
        /// Checks if the given itag is known.
        /// </summary>
        public static bool IsKnown(int itag)
        {
            return ItagMap.ContainsKey(itag);
        }
    }

    internal interface ICipherOperation
    {
        string Decipher(string input);
    }

    internal class PlayerSource
    {
        public List<ICipherOperation> CipherOperations { get; }

        public PlayerSource(List<ICipherOperation> cipherOperations)
        {
            CipherOperations = cipherOperations;
        }

        public string Decipher(string input)
        {
            foreach (var operation in CipherOperations)
                input = operation.Decipher(input);
            return input;
        }
    }

    /// <summary>
    /// Metadata associated with a certain <see cref="ClosedCaptionTrack"/>.
    /// </summary>
    public class ClosedCaptionTrackInfo
    {
        /// <summary>
        /// Manifest URL of the associated track.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Language of the associated track.
        /// </summary>
        public Language Language { get; }

        /// <summary>
        /// Whether the associated track was automatically generated.
        /// </summary>
        public bool IsAutoGenerated { get; }

        /// <summary />
        public ClosedCaptionTrackInfo(string url, Language language, bool isAutoGenerated)
        {
            Url = url;//.GuardNotNull(nameof(url));
            Language = language;//.GuardNotNull(nameof(language));
            IsAutoGenerated = isAutoGenerated;
        }

        /// <inheritdoc />
        public override string ToString() => $"{Language}";
    }

    /// <summary>
    /// Language information.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// ISO 639-1 code of this language.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Full English name of this language.
        /// </summary>
        public string Name { get; }

        /// <summary />
        public Language(string code, string name)
        {
            Code = code;//.GuardNotNull(nameof(code));
            Name = name;//.GuardNotNull(nameof(name));
        }

        /// <inheritdoc />
        public override string ToString() => $"{Code} ({Name})";
    }
}
