using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class test : MonoBehaviour {

	public Text mTxtConsole;

	private string token="";

	private StringBuilder consoleText = new StringBuilder("console\n");  

	private string hostname = "http://localhost:3000";

	public string testUUID = "testdeviceuuid";
	public string nickname = "nicknamehuh";

	void Start()
	{
		StartCoroutine(PostDevice(testUUID));
	}

	IEnumerator PostDevice(string uuid) {
		string url = string.Format("{0}/device", hostname);
		WWWForm form = new WWWForm();
		form.AddField("UUID", uuid);
		form.AddField("DeviceType", 10);

		WWW www = new WWW(url, form);

		yield return www;

		if(!string.IsNullOrEmpty(www.error)) 
		{
			PrintConsole(www.error);
		}
		else 
		{
			Hashtable decodeJSON;
			int errCode = CheckError(www.text, out decodeJSON);
			// if(errCode != 0)
			// 	DoErrorAction(errCode);
			// else
			// {
			// 	inputArea.SetActive(true);
			// }
		}
	}

	public void PostUser() {
		StartCoroutine(PostUser(testUUID, nickname, "ko-kr", 9));
	}

	IEnumerator PostUser(string uuid, string nickName, string locale, int offsettime) {
		string url = string.Format("{0}/user", hostname);
		WWWForm form = new WWWForm();
		form.AddField("UUID", uuid);
		form.AddField("NickName", nickName);
		form.AddField("Locale", locale);
		form.AddField("OffsetTime", offsettime);

		WWW www = new WWW(url, form);

		yield return www;

		if(!string.IsNullOrEmpty(www.error)) 
		{
			PrintConsole(www.error);
		}
		else 
		{
			Hashtable decodeJSON;
			int errCode = CheckError(www.text, out decodeJSON);
			// if(errCode != 0)
			// 	DoErrorAction(errCode);
			// else
			// {
			// 	inputArea.SetActive(true);
			// }
		}
	}

	public void GetToken() {
		StartCoroutine(GetToken(testUUID));
	}

	IEnumerator GetToken(string uuid) {
		string url = string.Format("{0}/device/token/{1}", hostname, uuid);

		WWW www = new WWW(url);

		yield return www;

		if(!string.IsNullOrEmpty(www.error)) 
		{
			PrintConsole(www.error);
		}
		else 
		{
			Hashtable decodeJSON;
			int errCode = CheckError(www.text, out decodeJSON);
			// if(errCode != 0)
			// 	DoErrorAction(errCode);
			// else
			// {
			// 	inputArea.SetActive(true);
			// }
		}
	}

	int CheckError(string JSONstr, out Hashtable decodeJSON) 
	{
		PrintConsole(JSONstr);
		decodeJSON = (Hashtable)MiniJSON.jsonDecode(JSONstr);
		int code = System.Convert.ToInt32(decodeJSON["result"]);
		switch(code) 
		{
			case 80101:
				PrintConsole("기기가 미등록된 상태입니다.");
				break;
			case 80202:
				PrintConsole("ID를 입력 후 사용자 등록이 필요합니다.");
				break;
			case 80204:
				PrintConsole("3~12글자 아이디를 입력하세요.");
				break;
			case 90101:
				PrintConsole("MagmaKick에 등록된 app이 아닙니다. appFingerPrint를 확인하세요.");
				break;
			case 966:
				PrintConsole("payload가 유효하지 않습니다.");
				break;
			case 967:
				PrintConsole("이미 사용된 payload입니다.");
				break;
			case 968:
				PrintConsole("developerPayload를 payload 필드로 첨부하세요");
				break;
		}
		return code;
	}

	void PrintConsole(string addText)
	{
		Debug.Log(addText);
		consoleText.AppendFormat("\n{0}",addText);
		mTxtConsole.text = consoleText.ToString();
	}
}
