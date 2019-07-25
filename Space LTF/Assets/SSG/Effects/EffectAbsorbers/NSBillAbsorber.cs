using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public  class NSBillAbsorber : BaseEffectAbsorber
{
    public NcBillboard NcBillboard;
    public override void Play()
    {
        NcBillboard.gameObject.SetActive(true);
        NcBillboard.Play();
        base.Play();
    }

    public override void Stop()
    {
        base.Stop();
        if (NcBillboard != null)
            NcBillboard.Stop();
    }
}
