using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class FollowLocationRigidbody : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_reference;

	private static readonly IntPtr NativeFieldInfoPtr_ownRigidbody;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_FixedUpdate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Transform reference
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reference);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reference)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Rigidbody ownRigidbody
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ownRigidbody);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Rigidbody>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ownRigidbody)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rigidbody));
		}
	}

	static FollowLocationRigidbody()
	{
		Il2CppClassPointerStore<FollowLocationRigidbody>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "FollowLocationRigidbody");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FollowLocationRigidbody>.NativeClassPtr);
		NativeFieldInfoPtr_reference = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FollowLocationRigidbody>.NativeClassPtr, "reference");
		NativeFieldInfoPtr_ownRigidbody = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FollowLocationRigidbody>.NativeClassPtr, "ownRigidbody");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FollowLocationRigidbody>.NativeClassPtr, 100664260);
		NativeMethodInfoPtr_FixedUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FollowLocationRigidbody>.NativeClassPtr, 100664261);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FollowLocationRigidbody>.NativeClassPtr, 100664262);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791967, XrefRangeEnd = 791971, MetadataInitTokenRva = 46442724L, MetadataInitFlagRva = 59827165L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791971, XrefRangeEnd = 791975, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(91)]
	[CachedScanResults(RefRangeStart = 37408, RefRangeEnd = 37499, XrefRangeStart = 37408, XrefRangeEnd = 37499, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FollowLocationRigidbody()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FollowLocationRigidbody>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FollowLocationRigidbody(IntPtr pointer)
		: base(pointer)
	{
	}
}
