using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailorNumberGuessingGame.Shared.Models;

public class Player
{
  public int Id { get; set; }

  [Required]
  public string Name { get; set; }

  [Required]
  public DateTime BirthDate { get; set; }
  
  public IList<Game> Games { get; set; }
}
