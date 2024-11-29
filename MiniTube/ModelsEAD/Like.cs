using System;
using System.Collections.Generic;

namespace MiniTube.ModelsEAD;

public partial class Like
{
    public int LikeId { get; set; }

    public int UserId { get; set; }

    public int VideoId { get; set; }

    public DateTime? LikedDate { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
