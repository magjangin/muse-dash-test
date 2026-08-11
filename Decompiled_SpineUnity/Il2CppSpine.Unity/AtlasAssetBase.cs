using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class AtlasAssetBase : ScriptableObject
{
	private static readonly IntPtr NativeMethodInfoPtr_get_PrimaryMaterial_Public_Abstract_Virtual_New_get_Material_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Materials_Public_Abstract_Virtual_New_get_IEnumerable_1_Material_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MaterialCount_Public_Abstract_Virtual_New_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsLoaded_Public_Abstract_Virtual_New_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Clear_Public_Abstract_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetAtlas_Public_Abstract_Virtual_New_Atlas_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe virtual Material PrimaryMaterial
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_PrimaryMaterial_Public_Abstract_Virtual_New_get_Material_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
	}

	public unsafe virtual IEnumerable<Material> Materials
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Materials_Public_Abstract_Virtual_New_get_IEnumerable_1_Material_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<Material>>(intPtr) : null;
		}
	}

	public unsafe virtual int MaterialCount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_MaterialCount_Public_Abstract_Virtual_New_get_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe virtual bool IsLoaded
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_IsLoaded_Public_Abstract_Virtual_New_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static AtlasAssetBase()
	{
		Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "AtlasAssetBase");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr);
		NativeMethodInfoPtr_get_PrimaryMaterial_Public_Abstract_Virtual_New_get_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr, 100663855);
		NativeMethodInfoPtr_get_Materials_Public_Abstract_Virtual_New_get_IEnumerable_1_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr, 100663856);
		NativeMethodInfoPtr_get_MaterialCount_Public_Abstract_Virtual_New_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr, 100663857);
		NativeMethodInfoPtr_get_IsLoaded_Public_Abstract_Virtual_New_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr, 100663858);
		NativeMethodInfoPtr_Clear_Public_Abstract_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr, 100663859);
		NativeMethodInfoPtr_GetAtlas_Public_Abstract_Virtual_New_Atlas_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr, 100663860);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr, 100663861);
	}

	[CallerCount(0)]
	public unsafe virtual void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Clear_Public_Abstract_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual Atlas GetAtlas()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetAtlas_Public_Abstract_Virtual_New_Atlas_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Atlas>(intPtr) : null;
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 18721, RefRangeEnd = 18752, XrefRangeStart = 18721, XrefRangeEnd = 18752, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AtlasAssetBase()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AtlasAssetBase>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AtlasAssetBase(IntPtr pointer)
		: base(pointer)
	{
	}
}
