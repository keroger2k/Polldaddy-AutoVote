using System;
using System.Threading;

namespace BabyPics {

  /// <summary>
  /// Contestant Names And Id's
  /// </summary>
  public enum Contestants {
    Hunter = 27549199,
    Brahm = 27549198
  }

  /// <summary>
  /// Program to simplify polldaddy voting.  The only thing used to track if you've voted or
  /// not are cookies, so this makes it so you don't need to worry about cookies.  
  /// 
  /// http://support.polldaddy.com/the-polldaddy-api/
  /// 
  /// </summary>
  public class Program {
    /// <summary>
    /// Hash that is genated from the main site.  This should be gotten dynamically
    /// but right now I'm lazy and you are required to enter it manually.
    /// </summary>
    private const string HASH = "ec3f306ac566c3022f36e00e85be9cf3";

    /// <summary>
    /// Poll Id that is being used.  Will have more than one answer.  Right now the
    /// contestant enum is being used to provide the answer_ids, but this could also be done
    /// dynamically.
    /// </summary>
    private const string POLL_ID = "6100952";

    /// <summary>
    /// Vote every 10 seconds.  Value in milliseconds.
    /// </summary>
    private const int TIMEOUT = 5500;
 
    static void Main(string[] args) {
      //loop for unlimited requests
      for (int i = 0; ; i++) {

        var prevoteRequest = new PreVoteData(HASH, POLL_ID);
        var voteRequest = new VoteData(POLL_ID, prevoteRequest.GetNonce(), (int)Contestants.Hunter);
        var match = voteRequest.GetMatchResults();

        if (match != null) {
          Console.WriteLine("Vote #{0}:", i);

          //display results
          Console.WriteLine("Match: {0} vs. {1}", match.Person1, match.Person2);
          Console.WriteLine("---------------------------------------");
          Console.WriteLine("{0} - {1}", match.Person1, match.Score1);
          Console.WriteLine("{0} - {1}", match.Person2, match.Score2);
          Console.WriteLine();
        }
        Thread.Sleep(TIMEOUT);
      }
    }
  }
}
