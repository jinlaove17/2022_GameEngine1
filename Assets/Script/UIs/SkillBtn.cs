using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class SkillBtn : MonoBehaviour 
{
    public Image skillFilter;
    public Text coolTimeCounter; //남은 쿨타임을 표시할 텍스트
 
    public float coolTime;
 
    private float _currentCoolTime; //남은 쿨타임을 추적 할 변수
 
    private bool _canUseSkill = true; //스킬을 사용할 수 있는지 확인하는 변수

    private void Start()
    {
        skillFilter.fillAmount = 0; //처음에 스킬 버튼을 가리지 않음
    }
 
    public void UseSkill()
    {
        if (_canUseSkill)
        {
            Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //스킬 버튼을 가림
            StartCoroutine(nameof(Cooltime));
 
            _currentCoolTime = coolTime;
            coolTimeCounter.text = "" + _currentCoolTime;
 
            StartCoroutine(nameof(CoolTimeCounter));
 
            _canUseSkill = false; //스킬을 사용하면 사용할 수 없는 상태로 바꿈
        }
        else
        {
            Debug.Log("아직 스킬을 사용할 수 없습니다.");
        }
    }
 
    IEnumerator Cooltime()
    {
        while(skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;
 
            yield return null;
        }
 
        _canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈
 
        yield break;
    }
 
    //남은 쿨타임을 계산할 코르틴을 만들어줍니다.
    IEnumerator CoolTimeCounter()
    {
        while(_currentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
 
            _currentCoolTime -= 1.0f;
            coolTimeCounter.text = "" + _currentCoolTime;
        }
 
        yield break;
    }
}