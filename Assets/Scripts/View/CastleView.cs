using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class CastleView : BuildingView
{
   [SerializeField] private GameObject[] _fractures;
   [SerializeField] private SpawnSliderView _spawnSliderView;   
   [SerializeField] private Transform _castle;
   [SerializeField] private Transform _bottomArenaFloor;
   [SerializeField] private Transform _shadowParent;

   [SerializeField] private Transform _hpBar;
   [SerializeField] private Transform _unitSpawnerPoints;
   
   [SerializeField] private Transform _marchPoint;
   [SerializeField] private Transform _marchPointMid;  

   [SerializeField] private Transform _marchPointDefensive;
   
   [SerializeField] private ParticleSystem Castle_Damage;
   
   [SerializeField] private ParticleSystem Castle_RuinedLow;
   [SerializeField] private ParticleSystem Castle_RuinedHigh;
   [SerializeField] private ParticleSystem Castle_Explosion;

   public void SetPoint(int direction)
   {
      if (direction == 1)
      {
         _castle.rotation = Quaternion.Euler(180, 0, -180);
         _castle.localPosition = new Vector3( _castle.localPosition.x,  _castle.localPosition.y, - _castle.localPosition.z);
         _bottomArenaFloor.rotation = Quaternion.Euler(180, 0, -180);
         _bottomArenaFloor.localPosition = new Vector3( _bottomArenaFloor.localPosition.x,  _bottomArenaFloor.localPosition.y, - _bottomArenaFloor.localPosition.z);
         _hpBar.rotation = Quaternion.Euler(0, 0, 0);
         _hpBar.localPosition = new Vector3( _hpBar.localPosition.x,  _hpBar.localPosition.y, - _hpBar.localPosition.z);
         _shadowParent.rotation = Quaternion.Euler(0, 0, 0);
      }
      else
      {
         _marchPoint = _marchPointDefensive;
      }

   }

   public Vector3 GetCastleRotation()
   {
      return _castle.rotation.eulerAngles;
   }
   public void SetSpawnSliderTimer(float time)
   {
      _spawnSliderView.SetTimer(time);
   }

   public void SetSpawnSliderValue(float value)
   {
      _spawnSliderView.SetSliderValue(value);
   }
   public void DamageCastle()
   {
      Castle_Damage.Play();   
   }

    public void RuinedLowCastle()
   {
      Castle_RuinedLow.gameObject.SetActive(true);    
      Castle_RuinedLow.Play();
   }

   public void RuinedHighCastle()
   {
      Castle_RuinedLow.gameObject.SetActive(false);    
      Castle_RuinedHigh.gameObject.SetActive(true);    
      Castle_RuinedHigh.Play();

   }

   public void CastleExplosion()
   {
      Castle_RuinedHigh.gameObject.SetActive(false);    
      Castle_Explosion.gameObject.SetActive(true);    
      Castle_Explosion.Play();
   }

   [ContextMenu("CreatFractures")]
   public void CreatFractures()
   {
      CreatFractures(3);
   }
   
   [ContextMenu("OpenModel")]
   public void OpenModel()
   {      
      SetLevelUpObject(3);
   }   

   public void CreatFractures(int level)
   {
      var fractures =  GameObject.Instantiate(_fractures[level], _castle.position, _castle.rotation);
     
      CloseModel();

      for (int i = 0; i < fractures.transform.childCount; i++)
      {
         var child = fractures.transform.GetChild(i);
         
         for (int j = 0; j < child.childCount; j++)
         {
            MeshRenderer meshRenderer = child.GetChild(j).gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = teamMat;
            Rigidbody rigidbody = child.GetChild(j).gameObject.GetComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.AddForce(Vector3.up, ForceMode.Impulse);  
         }
         
      }
      Destroy(fractures.gameObject, 4f);

   }

   public Transform GetMarchPoint()
   {
      return _marchPoint;
   }

   public Transform GetMarchPointMid()
   {
      return _marchPointMid;
   }

   public Transform GetMarchPointDefensive()
   {
      return _marchPointDefensive;
   }

   public Transform GetUnitSpawnerPoints()
   {
      return _unitSpawnerPoints;
   }  

}


