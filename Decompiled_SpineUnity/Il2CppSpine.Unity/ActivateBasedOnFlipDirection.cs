using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class ActivateBasedOnFlipDirection : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_skeletonRenderer;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonGraphic;

	private static readonly IntPtr NativeFieldInfoPtr_activeOnNormalX;

	private static readonly IntPtr NativeFieldInfoPtr_activeOnFlippedX;

	private static readonly IntPtr NativeFieldInfoPtr_jointsNormalX;

	private static readonly IntPtr NativeFieldInfoPtr_jointsFlippedX;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonComponent;

	private static readonly IntPtr NativeFieldInfoPtr_wasFlippedXBefore;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_FixedUpdate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleFlip_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_ResetJointPositions_Private_Void_Il2CppReferenceArray_1_HingeJoint2D_0;

	private static readonly IntPtr NativeMethodInfoPtr_CompensateMovementAfterFlipX_Private_Void_Transform_Transform_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SkeletonRenderer skeletonRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonRenderer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRenderer));
		}
	}

	public unsafe SkeletonGraphic skeletonGraphic
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonGraphic);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonGraphic>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonGraphic)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonGraphic));
		}
	}

	public unsafe GameObject activeOnNormalX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeOnNormalX);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeOnNormalX)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe GameObject activeOnFlippedX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeOnFlippedX);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeOnFlippedX)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe Il2CppReferenceArray<HingeJoint2D> jointsNormalX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jointsNormalX);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<HingeJoint2D>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jointsNormalX)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<HingeJoint2D> jointsFlippedX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jointsFlippedX);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<HingeJoint2D>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jointsFlippedX)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe ISkeletonComponent skeletonComponent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonComponent);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ISkeletonComponent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonComponent)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonComponent));
		}
	}

	public unsafe bool wasFlippedXBefore
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasFlippedXBefore);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasFlippedXBefore)) = flag;
		}
	}

	static ActivateBasedOnFlipDirection()
	{
		Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "ActivateBasedOnFlipDirection");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr);
		NativeFieldInfoPtr_skeletonRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, "skeletonRenderer");
		NativeFieldInfoPtr_skeletonGraphic = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, "skeletonGraphic");
		NativeFieldInfoPtr_activeOnNormalX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, "activeOnNormalX");
		NativeFieldInfoPtr_activeOnFlippedX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, "activeOnFlippedX");
		NativeFieldInfoPtr_jointsNormalX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, "jointsNormalX");
		NativeFieldInfoPtr_jointsFlippedX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, "jointsFlippedX");
		NativeFieldInfoPtr_skeletonComponent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, "skeletonComponent");
		NativeFieldInfoPtr_wasFlippedXBefore = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, "wasFlippedXBefore");
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, 100664254);
		NativeMethodInfoPtr_FixedUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, 100664255);
		NativeMethodInfoPtr_HandleFlip_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, 100664256);
		NativeMethodInfoPtr_ResetJointPositions_Private_Void_Il2CppReferenceArray_1_HingeJoint2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, 100664257);
		NativeMethodInfoPtr_CompensateMovementAfterFlipX_Private_Void_Transform_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, 100664258);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr, 100664259);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791906, XrefRangeEnd = 791919, MetadataInitTokenRva = 47194172L, MetadataInitFlagRva = 59827086L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791919, XrefRangeEnd = 791923, MetadataInitTokenRva = 47194092L, MetadataInitFlagRva = 59827087L)]
	public unsafe void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791942, RefRangeEnd = 791943, XrefRangeStart = 791923, XrefRangeEnd = 791942, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleFlip(bool isFlippedX)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&isFlippedX);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleFlip_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 791954, RefRangeEnd = 791956, XrefRangeStart = 791943, XrefRangeEnd = 791954, MetadataInitTokenRva = 47194160L, MetadataInitFlagRva = 59827088L)]
	public unsafe void ResetJointPositions(Il2CppReferenceArray<HingeJoint2D> joints)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)joints);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetJointPositions_Private_Void_Il2CppReferenceArray_1_HingeJoint2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791956, XrefRangeEnd = 791967, MetadataInitTokenRva = 47194068L, MetadataInitFlagRva = 59827089L)]
	public unsafe void CompensateMovementAfterFlipX(Transform toActivate, Transform toDeactivate)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toActivate);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toDeactivate);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CompensateMovementAfterFlipX_Private_Void_Transform_Transform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(91)]
	[CachedScanResults(RefRangeStart = 37408, RefRangeEnd = 37499, XrefRangeStart = 37408, XrefRangeEnd = 37499, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ActivateBasedOnFlipDirection()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ActivateBasedOnFlipDirection>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ActivateBasedOnFlipDirection(IntPtr pointer)
		: base(pointer)
	{
	}
}
