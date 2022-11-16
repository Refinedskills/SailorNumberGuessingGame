using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailorNumberGuessingGame.Shared.Models;

public class Action
{
  public int Id { get; set; }
  [ForeignKey("PlayerId")]
  public int PlayerId { get; set; }
  public int GameId { get; set; }
  [ForeignKey("GameId")]
  public Game Game { get; set; }

  DateTime GuessedDate { get; set; }
  public string NumberEntered { get; set; }

  public int NumberOfShips { get; set;}
  public int NumberOfBuoys { get; set; }
}
