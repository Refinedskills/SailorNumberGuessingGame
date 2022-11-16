using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailorNumberGuessingGame.Shared.Models;

public class Game
{
  public int Id { get; set; }

  public int PlayerId { get; set; }

  [ForeignKey("PlayerId")]
  public Player Player { get; set; }

  [Required]
  public string ExpectedNumber { get; set; }

  [Required]
  public DateTime GameStarted { get; set; }

  public DateTime GameEnded { get; set; }

  public bool successful { get; set; }


  public IList<Action> Actions { get; set; }

}
