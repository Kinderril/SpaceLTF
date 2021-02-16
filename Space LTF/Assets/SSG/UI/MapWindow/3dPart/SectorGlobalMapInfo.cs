using System.Linq;
using TMPro;
using UnityEngine;

public class SectorGlobalMapInfo : MonoBehaviour
{
    public Transform MainGo;
    public BaseEffectAbsorber OpenEffect;
    public BaseEffectAbsorber CloseEffect;
    private bool UseLines = false;
    public GameObject VerticalLine;
    public GameObject HorizontalLine;
    private bool _sideMove = false;
    private Vector3 _minHorizontal;
    private Vector3 _maxHorizontal;
    private Vector3 _minVertical;
    private Vector3 _maxVertical;
    private float EndTimeMove;
    public float DeltaMove = 3f;
    public TextMeshPro Field;
    public Transform ScaledHolder;
    public MeshRenderer BackgroundRenderer;
    private SectorData _sector;
    private GalaxyData _data;
    public Color TargetColor = Color.red;
    public Color SimpleColor = Color.green;

    public void Init(SectorData sector, GalaxyData data, float offset)
    {
        EndTimeMove = MyExtensions.Random(0, DeltaMove);
        _sideMove = MyExtensions.IsTrueEqual();
        MainGo.gameObject.SetActive(false);
        _sector = sector;
        _sector.OnCompleteChange += OnCompleteChange;
        _data = data;
        var sectorSize = sector.Size * offset;
        var scl = sectorSize * 0.11f;
        //        var sclVect = new Vector3(scl, 0, scl);
        ScaledHolder.localScale = new Vector3(scl, 0, scl);
        HorizontalLine.SetActive(false);
        VerticalLine.SetActive(false);
        //        HorizontalLine.transform.localScale = new Vector3(HorizontalLine.transform.localScale.x, 0, scl);
        //        VerticalLine.transform.localScale = new Vector3(VerticalLine.transform.localScale.x, 0, scl);
        var halfOffse = new Vector3(offset / 2f, 0, offset / 2f);
        var min = new Vector3(sector.StartX * offset, 0, sector.StartZ * offset);
        var max = new Vector3(sector.StartX * offset + sectorSize, 0, sector.StartZ * offset + sectorSize);
        var center = (max + min) / 2f - halfOffse;
        Field.transform.localPosition = new Vector3(0, 4, sector.Size * 4);
        transform.localPosition = center;
        if (sector.IsCore)
        {
            UseLines = true;
        }
        var color = _sector.IsFinal ? TargetColor : _sector.IsCore ? TargetColor : SimpleColor;
        //        var color = sector.IsCore ? TargetColor : SimpleColor;
        Utils.CopyMaterials(BackgroundRenderer, color, "_TintColor");
        color.a = 1f;
        Field.color = color;
//        HorizontalLine.SetActive(false);
//        VerticalLine.SetActive(false);

        _minHorizontal = center - new Vector3(sectorSize / 2f, 0, 0);
        _maxHorizontal = center + new Vector3(sectorSize / 2f, 0, 0);

        _minVertical = center - new Vector3(0, 0, sectorSize / 2f);
        _maxVertical = center + new Vector3(0, 0, sectorSize / 2f);
    }

    private void OnCompleteChange()
    {
        UpdateField();
    }

    private void UpdateField()
    {
        var sectorCells = _data.GetAllContainersNotNull().Where(x => !(x.Data is GlobalMapNothing) && x.Data.SectorId == _sector.Id).ToList();
        string ss = "";
        if (_sector.IsFinal)
        {
            ss = Namings.Tag("FinalSector");
        }
        else
        {
            if (_sector.IsCore)
            {
                ss = Namings.Tag("CoreSector");
            }
            else
            {
                if (_sector is SectorDungeon)
                {

                    ss = Namings.Tag("SectorDungeon");
                }
                else
                {

                    ss = Namings.Tag("NotCoreSector");
                }
            }
        }
        var completedCount = sectorCells.Count(x => x.Data.Completed);
        var totalCount = sectorCells.Count;
        string txt;
        if (_sector.IsMy)
        {
            txt =$"{Namings.Tag("MySector") }\n {_sector.Name}" ;
        }
        else
        {

            if (_sector.CanConcuqer())
            {
                txt = $"{ss}\n{Namings.Tag("Completed")}:{completedCount}/{totalCount} \n {_sector.Name}";
            }
            else
            {
                txt = $"{ss}\n{_sector.Name}";
            }
        }

        Field.text = txt;
    }
    public void Select()
    {
        if (_sector.IsHide)
        {
            return;
        }

        UpdateField();
        MainGo.gameObject.SetActive(true);
        CloseEffect.enabled = false;
        OpenEffect.enabled = true;
        OpenEffect.Play();
//        HorizontalLine.SetActive(true);
//        VerticalLine.SetActive(true);
    }

    void OnDestroy()
    {
        if (_sector != null)
            _sector.OnCompleteChange -= OnCompleteChange;
    }

    public void UnSelect()
    {
        MainGo.gameObject.SetActive(false);
        OpenEffect.enabled = false;
        CloseEffect.enabled = true;
        CloseEffect.Play();
        HorizontalLine.SetActive(false);
        VerticalLine.SetActive(false);
    }

    void Update()
    {
        UpdateLines();
    }

    private void UpdateLines()
    {
        //        if (!UseLines)
        //        {
        //            return;
        //        }

        var remain = EndTimeMove - Time.time;
        if (remain < 0)
        {
            _sideMove = !_sideMove;
            EndTimeMove = Time.time + DeltaMove;
            remain = DeltaMove;
        }

        var percent = Mathf.Clamp01(remain / DeltaMove);
        if (_sideMove)
        {
            percent = 1f - percent;
        }
        var horizontal = Vector3.Lerp(_minHorizontal, _maxHorizontal, percent);
        var vertical = Vector3.Lerp(_minVertical, _maxVertical, percent);

        VerticalLine.transform.position = vertical;
        HorizontalLine.transform.position = horizontal;
    }
}
