using System;
using System.Collections.Generic;

namespace MiniTube.ModelsEAD;

public partial class Video
{
    public int VideoId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? Thumbnail { get; set; }

    public byte[]? VideoFile { get; set; }

    public string? Keyword1 { get; set; }

    public string? Keyword2 { get; set; }

    public string? Keyword3 { get; set; }

    public DateTime? UploadDate { get; set; }

    public int? CommentsCount { get; set; }

    public int? LikesCount { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual User User { get; set; } = null!;
}
