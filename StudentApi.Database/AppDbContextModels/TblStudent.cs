using System;
using System.Collections.Generic;

namespace StudentApi.Database.AppDbContextModels;

public partial class TblStudent
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string FatherName { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsDelete { get; set; }
}
