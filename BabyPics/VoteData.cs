using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace BabyPics {
  public class VoteData : PollDaddyRequest {

    /// <summary>
    /// Actual voting URL.
    /// 
    /// http://polls.polldaddy.com/vote-js.php?p=[poll_id]&a=[answer_ids]&o=[other_text]&cookie=[cookie_detected]&url=[embed_url]&n=[nonce]
    ///   [answer_ids] holds the values of the answers, comma separated if more than one answer i.e., multiple choice polls.
    ///   [other_text] holds the text of the other answer, if applicable, need to urlencode text.
    ///   [cookie_detected] holds value of whether the voter has voted before, 0 means new vote, 1 means repeat voter
    ///   [embed_url] holds the URL of where the poll is embedded (optional)
    ///   [nonce] holds value of the nonce, this is only valid for short period.
    ///   
    /// </summary>
    private const string VOTE_URL = "http://polls.polldaddy.com/vote-js.php?p={0}&a={1},&o=&va=16&cookie=0&n={2}";

    public bool VoteStatus { get; set; }
    

    public VoteData(string PollId, string Nonce, int AnswerId) {
      this.request = (HttpWebRequest)WebRequest.Create(
          string.Format(VOTE_URL,
          PollId,
          AnswerId,
          Nonce));
      SetDefaultHeaders();
    }


    public Match GetMatchResults() {
      var voteResponse = (HttpWebResponse)request.GetResponse();
      var voteStream = voteResponse.GetResponseStream();
      var voteReadStream = new StreamReader(voteStream, Encoding.UTF8);
      var currentMatch = new Match();

      var stats = voteReadStream.ReadToEnd();
      var status = stats.Contains("Thank you for voting!");
      if (!status) {
        if (stats.Contains("Thank you, we have already counted your vote.")) {
          Console.WriteLine("Voting failed!");
          return null;
        } else {
          Console.WriteLine("Unable to get nonce!");
          return null;
        }
      }

      var regex1 = "<span class=\"pds-answer-text\">";
      var index1a = stats.IndexOf(regex1) + regex1.Length + 1;
      var index1b = stats.IndexOf('<', index1a);
      currentMatch.Person1 = stats.Substring(index1a, index1b - index1a);

      var index2a = stats.IndexOf(regex1, index1b + 1) + regex1.Length + 1;
      var index2b = stats.IndexOf('<', index2a);
      currentMatch.Person2 = stats.Substring(index2a, index2b - index2a);

      var regex2 = "<span class=\"pds-feedback-per\">";
      var index3a = stats.IndexOf(regex2) + regex2.Length + 1;
      var index3b = stats.IndexOf('<', index3a);
      currentMatch.Score1 = stats.Substring(index3a, index3b - index3a).Replace("nbsp;", string.Empty);

      var index4a = stats.IndexOf(regex2, index3b + 1) + regex2.Length + 1;
      var index4b = stats.IndexOf('<', index4a);
      currentMatch.Score2 = stats.Substring(index4a, index4b - index4a).Replace("nbsp;", string.Empty);
      return currentMatch;
    }

    protected override void SetDefaultHeaders() {
      base.SetDefaultHeaders();
      request.Host = "polls.polldaddy.com";
    }
  }
}
