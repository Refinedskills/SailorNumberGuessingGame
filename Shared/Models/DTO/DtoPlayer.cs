using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Model = SailorNumberGuessingGame.Shared.Models;


namespace SailorNumberGuessingGame.Shared.Models.DTO
{
  public class DtoPlayer
  {

    public DtoPlayer(Model.Player? source)
    {
      if (source != null)
      {
        this.Id = source.Id;
        this.Name = source.Name;
        this.BirthDate = source.BirthDate;
      }
    }

    public DtoPlayer() { }

    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string Token { get; set; }
  }
}
