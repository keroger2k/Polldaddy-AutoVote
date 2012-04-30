using System;
using System.IO;
using System.Net;
using System.Text;

namespace BabyPics {
  public class PreVoteData : PollDaddyRequest {

    /// <summary>
    /// Pre-voting url that return the nonce.  Must send the hash in the GET request.
    /// </summary>
    private const string PREVOTE_URL = "http://polldaddy.com/n/{0}/{1}?{2}";
    private string nonce;


    public PreVoteData(string Hash, string PollId) {
      this.request = (HttpWebRequest)WebRequest.Create(
          string.Format(PREVOTE_URL,
            Hash,
            PollId,
            DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).Ticks.ToString().Substring(0, 13)
            ));
      SetDefaultHeaders();
    }


    protected override void SetDefaultHeaders() {
      base.SetDefaultHeaders();
      request.Host = "polldaddy.com";
    }

    public string GetNonce() {
      if (String.IsNullOrEmpty(this.nonce)) {
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream receiveStream = response.GetResponseStream();
        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

        var requestRead = readStream.ReadToEnd();
        var reply = requestRead.Split(';');

        response.Close();
        receiveStream.Close();
        readStream.Close();

        this.nonce = reply[0].Split('=')[1].Replace("'", string.Empty);
      }
      return this.nonce;
    }

  }
}
