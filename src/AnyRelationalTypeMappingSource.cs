using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Scaffolding;

[SuppressMessage("Avoid uninstantiated internal classes", "CA1812", Justification = "It's instantiated through dependency injection")]
internal sealed class AnyRelationalTypeMappingSource(IRelationalTypeMappingSource mappingSource) : IRelationalTypeMappingSource
{
    CoreTypeMapping? ITypeMappingSource.FindMapping(IProperty property) => ((ITypeMappingSource)this).FindMapping(property);
#pragma warning disable CS0618 // Type or member is obsolete
    CoreTypeMapping? ITypeMappingSource.FindMapping(MemberInfo member) => ((ITypeMappingSource)this).FindMapping(member);
#pragma warning restore CS0618 // Type or member is obsolete
    CoreTypeMapping? ITypeMappingSource.FindMapping(Type type) => ((ITypeMappingSource)this).FindMapping(type);
    CoreTypeMapping? ITypeMappingSource.FindMapping(Type type, IModel model, CoreTypeMapping? elementMapping) => ((ITypeMappingSource)this).FindMapping(type, model, elementMapping);

    public RelationalTypeMapping? FindMapping(MemberInfo member) => mappingSource.FindMapping(member);
    public RelationalTypeMapping? FindMapping(Type type) => mappingSource.FindMapping(type);
    public RelationalTypeMapping? FindMapping(Type type, IModel model, CoreTypeMapping? elementMapping) => mappingSource.FindMapping(type, model, elementMapping);
    public RelationalTypeMapping? FindMapping(string storeTypeName) => mappingSource.FindMapping(storeTypeName);
    public RelationalTypeMapping? FindMapping(IProperty property) => mappingSource.FindMapping(property);
    public CoreTypeMapping? FindMapping(IElementType elementType) => mappingSource.FindMapping(elementType);
    public CoreTypeMapping? FindMapping(MemberInfo member, IModel model, bool useAttributes) => mappingSource.FindMapping(member, model, useAttributes);

    public RelationalTypeMapping FindMapping(Type type, string? storeTypeName, bool keyOrIndex, bool? unicode, int? size, bool? rowVersion, bool? fixedLength, int? precision, int? scale)
    {
        var mapping = mappingSource.FindMapping(type, storeTypeName, keyOrIndex, unicode, size, rowVersion, fixedLength, precision, scale);
        return mapping ?? new AnyRelationalTypeMapping(type, storeTypeName ?? "");
    }

    private sealed class AnyRelationalTypeMapping(Type type, string storeType) : RelationalTypeMapping(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(type), storeType))
    {
        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) => new AnyRelationalTypeMapping(type, StoreType);
    }
}
