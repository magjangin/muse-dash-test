using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppSpine.Unity;

public class SkeletonRenderer : MonoBehaviour
{
	[System.Serializable]
	public class SpriteMaskInteractionMaterials : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_materialsMaskDisabled;

		private static readonly System.IntPtr NativeFieldInfoPtr_materialsInsideMask;

		private static readonly System.IntPtr NativeFieldInfoPtr_materialsOutsideMask;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_AnyMaterialCreated_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Il2CppReferenceArray<Material> materialsMaskDisabled
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materialsMaskDisabled);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materialsMaskDisabled)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe Il2CppReferenceArray<Material> materialsInsideMask
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materialsInsideMask);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materialsInsideMask)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe Il2CppReferenceArray<Material> materialsOutsideMask
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materialsOutsideMask);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materialsOutsideMask)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe bool AnyMaterialCreated
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AnyMaterialCreated_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static SpriteMaskInteractionMaterials()
		{
			Il2CppClassPointerStore<SpriteMaskInteractionMaterials>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "SpriteMaskInteractionMaterials");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpriteMaskInteractionMaterials>.NativeClassPtr);
			NativeFieldInfoPtr_materialsMaskDisabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpriteMaskInteractionMaterials>.NativeClassPtr, "materialsMaskDisabled");
			NativeFieldInfoPtr_materialsInsideMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpriteMaskInteractionMaterials>.NativeClassPtr, "materialsInsideMask");
			NativeFieldInfoPtr_materialsOutsideMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpriteMaskInteractionMaterials>.NativeClassPtr, "materialsOutsideMask");
			NativeMethodInfoPtr_get_AnyMaterialCreated_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpriteMaskInteractionMaterials>.NativeClassPtr, 100664223);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpriteMaskInteractionMaterials>.NativeClassPtr, 100664224);
		}

		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 791312, RefRangeEnd = 791315, XrefRangeStart = 791301, XrefRangeEnd = 791312, MetadataInitTokenRva = 46304908L, MetadataInitFlagRva = 59848166L)]
		public unsafe SpriteMaskInteractionMaterials()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SpriteMaskInteractionMaterials>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SpriteMaskInteractionMaterials(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public sealed class InstructionDelegate : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonRendererInstruction_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonRendererInstruction_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static InstructionDelegate()
		{
			Il2CppClassPointerStore<InstructionDelegate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "InstructionDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstructionDelegate>.NativeClassPtr, 100664225);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonRendererInstruction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstructionDelegate>.NativeClassPtr, 100664226);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonRendererInstruction_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstructionDelegate>.NativeClassPtr, 100664227);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstructionDelegate>.NativeClassPtr, 100664228);
		}

		[CallerCount(6192)]
		[CachedScanResults(RefRangeStart = 39733, RefRangeEnd = 45925, XrefRangeStart = 39733, XrefRangeEnd = 45925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe InstructionDelegate(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InstructionDelegate>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(74)]
		[CachedScanResults(RefRangeStart = 171499, RefRangeEnd = 171573, XrefRangeStart = 171499, XrefRangeEnd = 171573, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Invoke(SkeletonRendererInstruction instruction)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instruction);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonRendererInstruction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 47234, RefRangeEnd = 47236, XrefRangeStart = 47234, XrefRangeEnd = 47236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(SkeletonRendererInstruction instruction, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instruction);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonRendererInstruction_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 45955, RefRangeEnd = 45956, XrefRangeStart = 45955, XrefRangeEnd = 45956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public InstructionDelegate(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator InstructionDelegate(System.Action<SkeletonRendererInstruction> P_0)
		{
			return DelegateSupport.ConvertDelegate<InstructionDelegate>((System.Delegate)P_0);
		}

		public static InstructionDelegate operator +(InstructionDelegate P_0, InstructionDelegate P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<InstructionDelegate>();
		}

		public static InstructionDelegate operator -(InstructionDelegate P_0, InstructionDelegate P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<InstructionDelegate>();
			}
			return (InstructionDelegate)obj;
		}
	}

	public sealed class SkeletonRendererDelegate : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonRenderer_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonRenderer_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static SkeletonRendererDelegate()
		{
			Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "SkeletonRendererDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr, 100664229);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr, 100664230);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonRenderer_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr, 100664231);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr, 100664232);
		}

		[CallerCount(6192)]
		[CachedScanResults(RefRangeStart = 39733, RefRangeEnd = 45925, XrefRangeStart = 39733, XrefRangeEnd = 45925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SkeletonRendererDelegate(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(74)]
		[CachedScanResults(RefRangeStart = 171499, RefRangeEnd = 171573, XrefRangeStart = 171499, XrefRangeEnd = 171573, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Invoke(SkeletonRenderer skeletonRenderer)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRenderer);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 47234, RefRangeEnd = 47236, XrefRangeStart = 47234, XrefRangeEnd = 47236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(SkeletonRenderer skeletonRenderer, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRenderer);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonRenderer_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 45955, RefRangeEnd = 45956, XrefRangeStart = 45955, XrefRangeEnd = 45956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SkeletonRendererDelegate(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator SkeletonRendererDelegate(System.Action<SkeletonRenderer> P_0)
		{
			return DelegateSupport.ConvertDelegate<SkeletonRendererDelegate>((System.Delegate)P_0);
		}

		public static SkeletonRendererDelegate operator +(SkeletonRendererDelegate P_0, SkeletonRendererDelegate P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<SkeletonRendererDelegate>();
		}

		public static SkeletonRendererDelegate operator -(SkeletonRendererDelegate P_0, SkeletonRendererDelegate P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<SkeletonRendererDelegate>();
			}
			return (SkeletonRendererDelegate)obj;
		}
	}

	[ObfuscatedName("Spine.Unity.SkeletonRenderer+<>c__DisplayClass75_0")]
	public sealed class __c__DisplayClass75_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_startsWith;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__FindAndApplySeparatorSlots_b__0_Internal_Boolean_String_0;

		public unsafe string startsWith
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startsWith);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startsWith)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static __c__DisplayClass75_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "<>c__DisplayClass75_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr);
			NativeFieldInfoPtr_startsWith = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr, "startsWith");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr, 100664233);
			NativeMethodInfoPtr__FindAndApplySeparatorSlots_b__0_Internal_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr, 100664234);
		}

		[CallerCount(2392)]
		[CachedScanResults(RefRangeStart = 18875, RefRangeEnd = 21267, XrefRangeStart = 18875, XrefRangeEnd = 21267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass75_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791315, XrefRangeEnd = 791317, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _FindAndApplySeparatorSlots_b__0(string slotName)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(slotName);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__FindAndApplySeparatorSlots_b__0_Internal_Boolean_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass75_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_NewSpineGameObject_Public_Static_T_SkeletonDataAsset_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_NewSpineGameObject_Public_Static_T_SkeletonDataAsset_0, Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_AddSpineComponent_Public_Static_T_GameObject_SkeletonDataAsset_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_AddSpineComponent_Public_Static_T_GameObject_SkeletonDataAsset_0, Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_skeletonDataAsset;

	private static readonly System.IntPtr NativeFieldInfoPtr_initialSkinName;

	private static readonly System.IntPtr NativeFieldInfoPtr_initialFlipX;

	private static readonly System.IntPtr NativeFieldInfoPtr_initialFlipY;

	private static readonly System.IntPtr NativeFieldInfoPtr_updateMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_updateWhenInvisible;

	private static readonly System.IntPtr NativeFieldInfoPtr_separatorSlotNames;

	private static readonly System.IntPtr NativeFieldInfoPtr_separatorSlots;

	private static readonly System.IntPtr NativeFieldInfoPtr_zSpacing;

	private static readonly System.IntPtr NativeFieldInfoPtr_useClipping;

	private static readonly System.IntPtr NativeFieldInfoPtr_immutableTriangles;

	private static readonly System.IntPtr NativeFieldInfoPtr_pmaVertexColors;

	private static readonly System.IntPtr NativeFieldInfoPtr_clearStateOnDisable;

	private static readonly System.IntPtr NativeFieldInfoPtr_tintBlack;

	private static readonly System.IntPtr NativeFieldInfoPtr_singleSubmesh;

	private static readonly System.IntPtr NativeFieldInfoPtr_fixDrawOrder;

	private static readonly System.IntPtr NativeFieldInfoPtr_addNormals;

	private static readonly System.IntPtr NativeFieldInfoPtr_calculateTangents;

	private static readonly System.IntPtr NativeFieldInfoPtr_maskInteraction;

	private static readonly System.IntPtr NativeFieldInfoPtr_maskMaterials;

	private static readonly System.IntPtr NativeFieldInfoPtr_STENCIL_COMP_PARAM_ID;

	private static readonly System.IntPtr NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_NONE;

	private static readonly System.IntPtr NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE;

	private static readonly System.IntPtr NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE;

	private static readonly System.IntPtr NativeFieldInfoPtr_disableRenderingOnOverride;

	private static readonly System.IntPtr NativeFieldInfoPtr_generateMeshOverride;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnPostProcessVertices;

	private static readonly System.IntPtr NativeFieldInfoPtr_customMaterialOverride;

	private static readonly System.IntPtr NativeFieldInfoPtr_customSlotMaterials;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentInstructions;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshGenerator;

	private static readonly System.IntPtr NativeFieldInfoPtr_rendererBuffers;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshRenderer;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshFilter;

	private static readonly System.IntPtr NativeFieldInfoPtr_valid;

	private static readonly System.IntPtr NativeFieldInfoPtr_skeleton;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnRebuild;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnMeshAndMaterialsUpdated;

	private static readonly System.IntPtr NativeFieldInfoPtr_reusedPropertyBlock;

	private static readonly System.IntPtr NativeFieldInfoPtr_SUBMESH_DUMMY_PARAM_ID;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UpdateMode_Public_get_UpdateMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UpdateMode_Public_set_Void_UpdateMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_generateMeshOverride_Private_add_Void_InstructionDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_generateMeshOverride_Private_rem_Void_InstructionDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_GenerateMeshOverride_Public_add_Void_InstructionDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_GenerateMeshOverride_Public_rem_Void_InstructionDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnPostProcessVertices_Public_add_Void_MeshGeneratorDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnPostProcessVertices_Public_rem_Void_MeshGeneratorDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CustomMaterialOverride_Public_get_Dictionary_2_Material_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CustomSlotMaterials_Public_get_Dictionary_2_Slot_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Skeleton_Public_Virtual_Final_New_get_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnRebuild_Public_add_Void_SkeletonRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnRebuild_Public_rem_Void_SkeletonRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SkeletonDataAsset_Public_Virtual_Final_New_get_SkeletonDataAsset_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewSpineGameObject_Public_Static_T_SkeletonDataAsset_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddSpineComponent_Public_Static_T_GameObject_SkeletonDataAsset_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMeshSettings_Public_Void_Settings_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearState_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnsureMeshGeneratorCapacity_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Virtual_New_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LateUpdate_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnBecameVisible_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnBecameInvisible_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindAndApplySeparatorSlots_Public_Void_String_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindAndApplySeparatorSlots_Public_Void_Func_2_String_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReapplySeparatorSlotNames_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AssignSpriteMaskMaterials_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitSpriteMaskMaterialsInsideMask_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitSpriteMaskMaterialsOutsideMask_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitSpriteMaskMaterialsForMaskType_Private_Boolean_CompareFunction_byref_Il2CppReferenceArray_1_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMaterialSettingsToFixDrawOrder_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SkeletonDataAsset skeletonDataAsset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataAsset);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonDataAsset>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataAsset)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset));
		}
	}

	public unsafe string initialSkinName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialSkinName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialSkinName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool initialFlipX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialFlipX);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialFlipX)) = flag;
		}
	}

	public unsafe bool initialFlipY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialFlipY);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialFlipY)) = flag;
		}
	}

	public unsafe UpdateMode updateMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateMode);
			return *(UpdateMode*)num;
		}
		set
		{
			*(UpdateMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateMode)) = updateMode;
		}
	}

	public unsafe UpdateMode updateWhenInvisible
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateWhenInvisible);
			return *(UpdateMode*)num;
		}
		set
		{
			*(UpdateMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateWhenInvisible)) = updateMode;
		}
	}

	public unsafe Il2CppStringArray separatorSlotNames
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorSlotNames);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorSlotNames)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe List<Slot> separatorSlots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorSlots);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Slot>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorSlots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe float zSpacing
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_zSpacing);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_zSpacing)) = num;
		}
	}

	public unsafe bool useClipping
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_useClipping);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_useClipping)) = flag;
		}
	}

	public unsafe bool immutableTriangles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_immutableTriangles);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_immutableTriangles)) = flag;
		}
	}

	public unsafe bool pmaVertexColors
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pmaVertexColors);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pmaVertexColors)) = flag;
		}
	}

	public unsafe bool clearStateOnDisable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clearStateOnDisable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clearStateOnDisable)) = flag;
		}
	}

	public unsafe bool tintBlack
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tintBlack);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tintBlack)) = flag;
		}
	}

	public unsafe bool singleSubmesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_singleSubmesh);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_singleSubmesh)) = flag;
		}
	}

	public unsafe bool fixDrawOrder
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fixDrawOrder);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fixDrawOrder)) = flag;
		}
	}

	public unsafe bool addNormals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_addNormals);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_addNormals)) = flag;
		}
	}

	public unsafe bool calculateTangents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_calculateTangents);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_calculateTangents)) = flag;
		}
	}

	public unsafe SpriteMaskInteraction maskInteraction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maskInteraction);
			return *(SpriteMaskInteraction*)num;
		}
		set
		{
			*(SpriteMaskInteraction*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maskInteraction)) = spriteMaskInteraction;
		}
	}

	public unsafe SpriteMaskInteractionMaterials maskMaterials
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maskMaterials);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SpriteMaskInteractionMaterials>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maskMaterials)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spriteMaskInteractionMaterials));
		}
	}

	public unsafe static int STENCIL_COMP_PARAM_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_STENCIL_COMP_PARAM_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_STENCIL_COMP_PARAM_ID, (void*)(&num));
		}
	}

	public unsafe static CompareFunction STENCIL_COMP_MASKINTERACTION_NONE
	{
		get
		{
			Unsafe.SkipInit(out CompareFunction result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_NONE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_NONE, (void*)(&compareFunction));
		}
	}

	public unsafe static CompareFunction STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE
	{
		get
		{
			Unsafe.SkipInit(out CompareFunction result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE, (void*)(&compareFunction));
		}
	}

	public unsafe static CompareFunction STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE
	{
		get
		{
			Unsafe.SkipInit(out CompareFunction result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE, (void*)(&compareFunction));
		}
	}

	public unsafe bool disableRenderingOnOverride
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_disableRenderingOnOverride);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_disableRenderingOnOverride)) = flag;
		}
	}

	public unsafe InstructionDelegate generateMeshOverride
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_generateMeshOverride);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<InstructionDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_generateMeshOverride)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instructionDelegate));
		}
	}

	public unsafe MeshGeneratorDelegate OnPostProcessVertices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnPostProcessVertices);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshGeneratorDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnPostProcessVertices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshGeneratorDelegate));
		}
	}

	public unsafe Dictionary<Material, Material> customMaterialOverride
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customMaterialOverride);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Material, Material>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customMaterialOverride)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<Slot, Material> customSlotMaterials
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customSlotMaterials);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Slot, Material>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customSlotMaterials)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe SkeletonRendererInstruction currentInstructions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentInstructions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRendererInstruction>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentInstructions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRendererInstruction));
		}
	}

	public unsafe MeshGenerator meshGenerator
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshGenerator);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshGenerator>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshGenerator)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshGenerator));
		}
	}

	public unsafe MeshRendererBuffers rendererBuffers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rendererBuffers);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshRendererBuffers>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rendererBuffers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRendererBuffers));
		}
	}

	public unsafe MeshRenderer meshRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
		}
	}

	public unsafe MeshFilter meshFilter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshFilter);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshFilter>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshFilter)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshFilter));
		}
	}

	public unsafe bool valid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_valid);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_valid)) = flag;
		}
	}

	public unsafe Skeleton skeleton
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeleton);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skeleton>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeleton)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton));
		}
	}

	public unsafe SkeletonRendererDelegate OnRebuild
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnRebuild);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRendererDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnRebuild)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRendererDelegate));
		}
	}

	public unsafe SkeletonRendererDelegate OnMeshAndMaterialsUpdated
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnMeshAndMaterialsUpdated);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRendererDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnMeshAndMaterialsUpdated)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRendererDelegate));
		}
	}

	public unsafe MaterialPropertyBlock reusedPropertyBlock
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reusedPropertyBlock);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MaterialPropertyBlock>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reusedPropertyBlock)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertyBlock));
		}
	}

	public unsafe static int SUBMESH_DUMMY_PARAM_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SUBMESH_DUMMY_PARAM_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SUBMESH_DUMMY_PARAM_ID, (void*)(&num));
		}
	}

	public unsafe UpdateMode UpdateMode
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UpdateMode_Public_get_UpdateMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(UpdateMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UpdateMode_Public_set_Void_UpdateMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<Material, Material> CustomMaterialOverride
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CustomMaterialOverride_Public_get_Dictionary_2_Material_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Material, Material>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<Slot, Material> CustomSlotMaterials
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CustomSlotMaterials_Public_get_Dictionary_2_Slot_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Slot, Material>>(intPtr) : null;
		}
	}

	public unsafe virtual Skeleton Skeleton
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 791346, RefRangeEnd = 791351, XrefRangeStart = 791346, XrefRangeEnd = 791346, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Skeleton_Public_Virtual_Final_New_get_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skeleton>(intPtr) : null;
		}
	}

	public unsafe virtual SkeletonDataAsset SkeletonDataAsset
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonDataAsset_Public_Virtual_Final_New_get_SkeletonDataAsset_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonDataAsset>(intPtr) : null;
		}
	}

	static SkeletonRenderer()
	{
		Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonRenderer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr);
		NativeFieldInfoPtr_skeletonDataAsset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "skeletonDataAsset");
		NativeFieldInfoPtr_initialSkinName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "initialSkinName");
		NativeFieldInfoPtr_initialFlipX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "initialFlipX");
		NativeFieldInfoPtr_initialFlipY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "initialFlipY");
		NativeFieldInfoPtr_updateMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "updateMode");
		NativeFieldInfoPtr_updateWhenInvisible = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "updateWhenInvisible");
		NativeFieldInfoPtr_separatorSlotNames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "separatorSlotNames");
		NativeFieldInfoPtr_separatorSlots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "separatorSlots");
		NativeFieldInfoPtr_zSpacing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "zSpacing");
		NativeFieldInfoPtr_useClipping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "useClipping");
		NativeFieldInfoPtr_immutableTriangles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "immutableTriangles");
		NativeFieldInfoPtr_pmaVertexColors = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "pmaVertexColors");
		NativeFieldInfoPtr_clearStateOnDisable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "clearStateOnDisable");
		NativeFieldInfoPtr_tintBlack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "tintBlack");
		NativeFieldInfoPtr_singleSubmesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "singleSubmesh");
		NativeFieldInfoPtr_fixDrawOrder = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "fixDrawOrder");
		NativeFieldInfoPtr_addNormals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "addNormals");
		NativeFieldInfoPtr_calculateTangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "calculateTangents");
		NativeFieldInfoPtr_maskInteraction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "maskInteraction");
		NativeFieldInfoPtr_maskMaterials = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "maskMaterials");
		NativeFieldInfoPtr_STENCIL_COMP_PARAM_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "STENCIL_COMP_PARAM_ID");
		NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_NONE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "STENCIL_COMP_MASKINTERACTION_NONE");
		NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE");
		NativeFieldInfoPtr_STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE");
		NativeFieldInfoPtr_disableRenderingOnOverride = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "disableRenderingOnOverride");
		NativeFieldInfoPtr_generateMeshOverride = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "generateMeshOverride");
		NativeFieldInfoPtr_OnPostProcessVertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "OnPostProcessVertices");
		NativeFieldInfoPtr_customMaterialOverride = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "customMaterialOverride");
		NativeFieldInfoPtr_customSlotMaterials = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "customSlotMaterials");
		NativeFieldInfoPtr_currentInstructions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "currentInstructions");
		NativeFieldInfoPtr_meshGenerator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "meshGenerator");
		NativeFieldInfoPtr_rendererBuffers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "rendererBuffers");
		NativeFieldInfoPtr_meshRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "meshRenderer");
		NativeFieldInfoPtr_meshFilter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "meshFilter");
		NativeFieldInfoPtr_valid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "valid");
		NativeFieldInfoPtr_skeleton = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "skeleton");
		NativeFieldInfoPtr_OnRebuild = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "OnRebuild");
		NativeFieldInfoPtr_OnMeshAndMaterialsUpdated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "OnMeshAndMaterialsUpdated");
		NativeFieldInfoPtr_reusedPropertyBlock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "reusedPropertyBlock");
		NativeFieldInfoPtr_SUBMESH_DUMMY_PARAM_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, "SUBMESH_DUMMY_PARAM_ID");
		NativeMethodInfoPtr_get_UpdateMode_Public_get_UpdateMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664185);
		NativeMethodInfoPtr_set_UpdateMode_Public_set_Void_UpdateMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664186);
		NativeMethodInfoPtr_add_generateMeshOverride_Private_add_Void_InstructionDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664187);
		NativeMethodInfoPtr_remove_generateMeshOverride_Private_rem_Void_InstructionDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664188);
		NativeMethodInfoPtr_add_GenerateMeshOverride_Public_add_Void_InstructionDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664189);
		NativeMethodInfoPtr_remove_GenerateMeshOverride_Public_rem_Void_InstructionDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664190);
		NativeMethodInfoPtr_add_OnPostProcessVertices_Public_add_Void_MeshGeneratorDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664191);
		NativeMethodInfoPtr_remove_OnPostProcessVertices_Public_rem_Void_MeshGeneratorDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664192);
		NativeMethodInfoPtr_get_CustomMaterialOverride_Public_get_Dictionary_2_Material_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664193);
		NativeMethodInfoPtr_get_CustomSlotMaterials_Public_get_Dictionary_2_Slot_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664194);
		NativeMethodInfoPtr_get_Skeleton_Public_Virtual_Final_New_get_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664195);
		NativeMethodInfoPtr_add_OnRebuild_Public_add_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664196);
		NativeMethodInfoPtr_remove_OnRebuild_Public_rem_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664197);
		NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664198);
		NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664199);
		NativeMethodInfoPtr_get_SkeletonDataAsset_Public_Virtual_Final_New_get_SkeletonDataAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664200);
		NativeMethodInfoPtr_NewSpineGameObject_Public_Static_T_SkeletonDataAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664201);
		NativeMethodInfoPtr_AddSpineComponent_Public_Static_T_GameObject_SkeletonDataAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664202);
		NativeMethodInfoPtr_SetMeshSettings_Public_Void_Settings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664203);
		NativeMethodInfoPtr_Awake_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664204);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664205);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664206);
		NativeMethodInfoPtr_ClearState_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664207);
		NativeMethodInfoPtr_EnsureMeshGeneratorCapacity_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664208);
		NativeMethodInfoPtr_Initialize_Public_Virtual_New_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664209);
		NativeMethodInfoPtr_LateUpdate_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664210);
		NativeMethodInfoPtr_OnBecameVisible_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664211);
		NativeMethodInfoPtr_OnBecameInvisible_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664212);
		NativeMethodInfoPtr_FindAndApplySeparatorSlots_Public_Void_String_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664213);
		NativeMethodInfoPtr_FindAndApplySeparatorSlots_Public_Void_Func_2_String_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664214);
		NativeMethodInfoPtr_ReapplySeparatorSlotNames_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664215);
		NativeMethodInfoPtr_AssignSpriteMaskMaterials_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664216);
		NativeMethodInfoPtr_InitSpriteMaskMaterialsInsideMask_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664217);
		NativeMethodInfoPtr_InitSpriteMaskMaterialsOutsideMask_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664218);
		NativeMethodInfoPtr_InitSpriteMaskMaterialsForMaskType_Private_Boolean_CompareFunction_byref_Il2CppReferenceArray_1_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664219);
		NativeMethodInfoPtr_SetMaterialSettingsToFixDrawOrder_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664220);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr, 100664221);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791317, XrefRangeEnd = 791321, MetadataInitTokenRva = 46275672L, MetadataInitFlagRva = 59848147L)]
	public unsafe void add_generateMeshOverride(InstructionDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_generateMeshOverride_Private_add_Void_InstructionDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791321, XrefRangeEnd = 791325, MetadataInitTokenRva = 46275836L, MetadataInitFlagRva = 59848148L)]
	public unsafe void remove_generateMeshOverride(InstructionDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_generateMeshOverride_Private_rem_Void_InstructionDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791325, XrefRangeEnd = 791330, MetadataInitTokenRva = 46275672L, MetadataInitFlagRva = 59848147L)]
	public unsafe void add_GenerateMeshOverride(InstructionDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_GenerateMeshOverride_Public_add_Void_InstructionDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 791335, RefRangeEnd = 791338, XrefRangeStart = 791330, XrefRangeEnd = 791335, MetadataInitTokenRva = 46275836L, MetadataInitFlagRva = 59848148L)]
	public unsafe void remove_GenerateMeshOverride(InstructionDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_GenerateMeshOverride_Public_rem_Void_InstructionDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791338, XrefRangeEnd = 791342, MetadataInitTokenRva = 46275608L, MetadataInitFlagRva = 59848149L)]
	public unsafe void add_OnPostProcessVertices(MeshGeneratorDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnPostProcessVertices_Public_add_Void_MeshGeneratorDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791342, XrefRangeEnd = 791346, MetadataInitTokenRva = 46275752L, MetadataInitFlagRva = 59848150L)]
	public unsafe void remove_OnPostProcessVertices(MeshGeneratorDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnPostProcessVertices_Public_rem_Void_MeshGeneratorDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 791355, RefRangeEnd = 791358, XrefRangeStart = 791351, XrefRangeEnd = 791355, MetadataInitTokenRva = 46275648L, MetadataInitFlagRva = 59848151L)]
	public unsafe void add_OnRebuild(SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnRebuild_Public_add_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 791362, RefRangeEnd = 791370, XrefRangeStart = 791358, XrefRangeEnd = 791362, MetadataInitTokenRva = 46275800L, MetadataInitFlagRva = 59848152L)]
	public unsafe void remove_OnRebuild(SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnRebuild_Public_rem_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791370, XrefRangeEnd = 791374, MetadataInitTokenRva = 46275560L, MetadataInitFlagRva = 59848153L)]
	public unsafe void add_OnMeshAndMaterialsUpdated(SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791374, XrefRangeEnd = 791378, MetadataInitTokenRva = 46275732L, MetadataInitFlagRva = 59848154L)]
	public unsafe void remove_OnMeshAndMaterialsUpdated(SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791385, RefRangeEnd = 791386, XrefRangeStart = 791378, XrefRangeEnd = 791385, MetadataInitTokenRva = 46275356L, MetadataInitFlagRva = 59946241L)]
	public unsafe static T NewSpineGameObject<T>(SkeletonDataAsset skeletonDataAsset) where T : SkeletonRenderer
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_NewSpineGameObject_Public_Static_T_SkeletonDataAsset_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791391, RefRangeEnd = 791392, XrefRangeStart = 791386, XrefRangeEnd = 791391, MetadataInitTokenRva = 46275032L, MetadataInitFlagRva = 59946240L)]
	public unsafe static T AddSpineComponent<T>(GameObject gameObject, SkeletonDataAsset skeletonDataAsset) where T : SkeletonRenderer
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_AddSpineComponent_Public_Static_T_GameObject_SkeletonDataAsset_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	public unsafe void SetMeshSettings(MeshGenerator.Settings settings)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&settings);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMeshSettings_Public_Void_Settings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791392, XrefRangeEnd = 791393, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791406, RefRangeEnd = 791407, XrefRangeStart = 791393, XrefRangeEnd = 791406, MetadataInitTokenRva = 46275120L, MetadataInitFlagRva = 59848155L)]
	public unsafe virtual void ClearState()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ClearState_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791407, XrefRangeEnd = 791408, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EnsureMeshGeneratorCapacity(int minimumVertexCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&minimumVertexCount);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureMeshGeneratorCapacity_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 791443, RefRangeEnd = 791446, XrefRangeStart = 791408, XrefRangeEnd = 791443, MetadataInitTokenRva = 46275296L, MetadataInitFlagRva = 59848156L)]
	public unsafe virtual void Initialize(bool overwrite)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&overwrite);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Initialize_Public_Virtual_New_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 791498, RefRangeEnd = 791500, XrefRangeStart = 791446, XrefRangeEnd = 791498, MetadataInitTokenRva = 46275320L, MetadataInitFlagRva = 59848157L)]
	public unsafe virtual void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_LateUpdate_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void OnBecameVisible()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnBecameVisible_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void OnBecameInvisible()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnBecameInvisible_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791500, XrefRangeEnd = 791512, MetadataInitTokenRva = 46275192L, MetadataInitFlagRva = 59848158L)]
	public unsafe void FindAndApplySeparatorSlots(string startsWith, bool clearExistingSeparators = true, bool updateStringArray = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(startsWith);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clearExistingSeparators;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &updateStringArray;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindAndApplySeparatorSlots_Public_Void_String_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791549, RefRangeEnd = 791550, XrefRangeStart = 791512, XrefRangeEnd = 791549, MetadataInitTokenRva = 46275144L, MetadataInitFlagRva = 59848159L)]
	public unsafe void FindAndApplySeparatorSlots(Il2CppSystem.Func<string, bool> slotNamePredicate, bool clearExistingSeparators = true, bool updateStringArray = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slotNamePredicate);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clearExistingSeparators;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &updateStringArray;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindAndApplySeparatorSlots_Public_Void_Func_2_String_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791550, XrefRangeEnd = 791556, MetadataInitTokenRva = 46275416L, MetadataInitFlagRva = 59848160L)]
	public unsafe void ReapplySeparatorSlotNames()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReapplySeparatorSlotNames_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791564, RefRangeEnd = 791565, XrefRangeStart = 791556, XrefRangeEnd = 791564, MetadataInitTokenRva = 46275068L, MetadataInitFlagRva = 59848161L)]
	public unsafe void AssignSpriteMaskMaterials()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AssignSpriteMaskMaterials_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791565, XrefRangeEnd = 791567, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool InitSpriteMaskMaterialsInsideMask()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitSpriteMaskMaterialsInsideMask_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791567, XrefRangeEnd = 791569, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool InitSpriteMaskMaterialsOutsideMask()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitSpriteMaskMaterialsOutsideMask_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 791583, RefRangeEnd = 791585, XrefRangeStart = 791569, XrefRangeEnd = 791583, MetadataInitTokenRva = 46275248L, MetadataInitFlagRva = 59848162L)]
	public unsafe bool InitSpriteMaskMaterialsForMaskType(CompareFunction maskFunction, ref Il2CppReferenceArray<Material> materialsToFill)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&maskFunction);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialsToFill);
		*(System.IntPtr**)num = &intPtr;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitSpriteMaskMaterialsForMaskType_Private_Boolean_CompareFunction_byref_Il2CppReferenceArray_1_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		System.IntPtr intPtr4 = intPtr;
		materialsToFill = ((intPtr4 == (System.IntPtr)0) ? null : new Il2CppReferenceArray<Material>(intPtr4));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791602, RefRangeEnd = 791603, XrefRangeStart = 791585, XrefRangeEnd = 791602, MetadataInitTokenRva = 46275432L, MetadataInitFlagRva = 59848163L)]
	public unsafe void SetMaterialSettingsToFixDrawOrder()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMaterialSettingsToFixDrawOrder_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 791639, RefRangeEnd = 791641, XrefRangeStart = 791603, XrefRangeEnd = 791639, MetadataInitTokenRva = 46275532L, MetadataInitFlagRva = 59848164L)]
	public unsafe SkeletonRenderer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonRenderer>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonRenderer(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
