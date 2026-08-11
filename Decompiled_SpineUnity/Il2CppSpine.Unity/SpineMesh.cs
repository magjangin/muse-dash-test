using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppSpine.Unity;

public static class SpineMesh : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_NewSkeletonMesh_Public_Static_Mesh_0;

	static SpineMesh()
	{
		Il2CppClassPointerStore<SpineMesh>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SpineMesh");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpineMesh>.NativeClassPtr);
		NativeMethodInfoPtr_NewSkeletonMesh_Public_Static_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineMesh>.NativeClassPtr, 100664380);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793268, RefRangeEnd = 793269, XrefRangeStart = 793260, XrefRangeEnd = 793268, MetadataInitTokenRva = 46303260L, MetadataInitFlagRva = 59848236L)]
	public unsafe static Mesh NewSkeletonMesh()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewSkeletonMesh_Public_Static_Mesh_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	public SpineMesh(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
