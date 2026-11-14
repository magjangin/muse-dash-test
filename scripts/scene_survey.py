import UnityPy, glob, os, collections, json, re, sys
UnityPy.config.FALLBACK_UNITY_VERSION="2019.4.41f1"
D=r"H:\muse dash hwa\MuseDash_Data\StreamingAssets\aa\StandaloneWindows64"
out={}
def survey(f):
    env=UnityPy.load(f)
    objs={o.path_id:o for o in env.objects}
    gos={}; tr={}; comps=collections.defaultdict(list)
    for pid,o in objs.items():
        t=o.type.name
        if t=="GameObject":
            d=o.read(); gos[pid]=d
            for c in d.m_Component:
                try: comps[pid].append(c.component.path_id)
                except: comps[pid].append(c["component"]["m_PathID"] if isinstance(c,dict) else c[1].path_id)
    # transform parents
    father={}; goOfTr={}; trOfGo={}
    for pid,o in objs.items():
        if o.type.name in("Transform","RectTransform"):
            d=o.read(); father[pid]=d.m_Father.path_id; goOfTr[pid]=d.m_GameObject.path_id; trOfGo[d.m_GameObject.path_id]=pid
    def path(gid):
        parts=[]; t=trOfGo.get(gid)
        while t:
            parts.append(gos[goOfTr[t]].m_Name if goOfTr[t] in gos else "?"); t=father.get(t)
            if t==0: break
        return "/".join(reversed(parts))
    scriptcache={}
    def cls(o):
        d=o.read()
        try:
            s=d.m_Script.read(); return s.m_ClassName
        except Exception as e: return "?mono"
    res={}
    clips={}
    for pid,o in objs.items():
        if o.type.name=="AnimationClip":
            d=o.read()
            ppt=len(getattr(d,"m_PPtrCurves",[]) or [])
            pos=[]
            for c in (getattr(d,"m_PositionCurves",[]) or []): pos.append(c.path)
            clips[d.m_Name]=dict(ppt=ppt,len=round(d.m_AnimationClipSettings.m_StopTime,2) if hasattr(d,"m_AnimationClipSettings") else None,loop=getattr(d,"m_AnimationClipSettings",None) and d.m_AnimationClipSettings.m_LoopTime,pos=len(pos),rot=len(getattr(d,"m_RotationCurves",[]) or []),scale=len(getattr(d,"m_ScaleCurves",[]) or []),color=len(getattr(d,"m_FloatCurves",[]) or []))
    for gid,g in gos.items():
        p=path(gid)
        if not p: continue
        root=p.split("/")[0]
        r=res.setdefault(root,dict(sprites=0,monos=collections.Counter(),spine=[],animators=0,particles=0,mesh=0,spritenames=collections.Counter(),children=set(),monoPaths=collections.defaultdict(list)))
        if "/" in p and p.count("/")==1: r["children"].add(p.split("/")[1])
        for cid in comps[gid]:
            o=objs.get(cid)
            if not o: continue
            t=o.type.name
            if t=="SpriteRenderer":
                r["sprites"]+=1
                try:
                    sp=o.read().m_Sprite.read().m_Name; r["spritenames"][sp]+=1
                except: pass
            elif t=="MonoBehaviour":
                c=cls(o); r["monos"][c]+=1
                if len(r["monoPaths"][c])<3: r["monoPaths"][c].append(p)
                if c=="SkeletonAnimation": r["spine"].append(p)
            elif t=="Animator": r["animators"]+=1
            elif t=="ParticleSystem": r["particles"]+=1
            elif t=="MeshRenderer": r["mesh"]+=1
    for k,r in res.items():
        r["monos"]=dict(r["monos"]); r["children"]=sorted(r["children"]); r["spritenames"]=dict(r["spritenames"].most_common(400)); r["monoPaths"]=dict(r["monoPaths"])
        r["nsprites_distinct"]=len(r["spritenames"])
    return res,clips
for f in sorted(glob.glob(D+r"\gamescene_*_assets_all_*.bundle")):
    n=os.path.basename(f).split("_assets_all")[0]
    try:
        res,clips=survey(f)
    except Exception as e:
        print(n,"ERR",e); continue
    out[n]=dict(roots={k:v for k,v in res.items() if k.lower().startswith("scene") or True},clips=clips)
    print(n,[ (k,v["sprites"],len(v["spine"])) for k,v in res.items() if k.lower().startswith("scene")],flush=True)
json.dump(out,open(os.path.join(os.environ.get("TEMP","."),"_scene_survey.json"),"w",encoding="utf8"),ensure_ascii=False,indent=1)

