using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imagine.Core.Entities;

public class BaseEntity
{
    public int Id { get; set; }
}