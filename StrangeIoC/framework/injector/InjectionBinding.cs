/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

/**
 * @class strange.extensions.injector.impl.InjectionBinding
 *
 * The Binding for Injections.
 *
 * @see strange.extensions.injector.api.IInjectionBinding
 */

using strange.extensions.injector.api;
using strange.framework.api;
using strange.framework.impl;
using System;

namespace strange.extensions.injector.impl
{
public class InjectionBinding : Binding, IInjectionBinding
{
	private InjectionBindingType _type = InjectionBindingType.DEFAULT;
	private bool _toInject = true;
	private bool _isCrossContext = false;

	public InjectionBinding( Binder.BindingResolver resolver )
	{
		this.resolver = resolver;
		keyConstraint = BindingConstraintType.MANY;
		valueConstraint = BindingConstraintType.ONE;
	}

	public InjectionBindingType type
	{
		get => _type;
		set => _type = value;
	}

	public bool toInject => _toInject;

	public IInjectionBinding ToInject( bool value )
	{
		_toInject = value;
		return this;
	}

	public bool isCrossContext => _isCrossContext;

	public IInjectionBinding ToSingleton()
	{
		//If already a value, this mapping is redundant
		if( type == InjectionBindingType.VALUE )
		{
			return this;
		}

		type = InjectionBindingType.SINGLETON;
		if( resolver != null )
		{
			resolver( this );
		}
		return this;
	}

	public IInjectionBinding ToValue( object o )
	{
		type = InjectionBindingType.VALUE;
		SetValue( o );
		return this;
	}

	public IInjectionBinding SetValue( object o )
	{
		if( o == null )
		{
			UnityEngine.Debug.LogError( "IInjectionBinding SetValue object == NULL" );
			return null;
		}

		Type objType = o.GetType();

		object[] keys = key as object[];
		int aa = keys.Length;
		//Check that value is legal for the provided keys
		for( int a = 0; a < aa; a++ )
		{
			object aKey = keys[a];
			Type keyType = ( aKey is Type ) ? aKey as Type : aKey.GetType();
			if( keyType.IsAssignableFrom( objType ) == false && ( HasGenericAssignableFrom( keyType, objType ) == false ) )
			{
				throw new InjectionException( "Injection cannot bind a value that does not extend or implement the binding type.", InjectionExceptionType.ILLEGAL_BINDING_VALUE );
			}
		}
		To( o );
		return this;
	}

       protected bool HasGenericAssignableFrom( Type keyType, Type objType )
       {
               //If the key is an open generic type, compare against its definition
               Type genericDefinition = keyType.IsGenericTypeDefinition ? keyType : keyType.GetGenericTypeDefinition();

               return IsGenericTypeAssignable( objType, genericDefinition );

       }

	protected bool IsGenericTypeAssignable( Type givenType, Type genericType )
	{
		Type[] interfaceTypes = givenType.GetInterfaces();

		foreach( Type it in interfaceTypes )
		{
			if( it.IsGenericType && it.GetGenericTypeDefinition() == genericType )
			{
				return true;
			}
		}

		if( givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType )
		{
			return true;
		}

		Type baseType = givenType.BaseType;
		if( baseType == null )
		{
			return false;
		}

		return IsGenericTypeAssignable( baseType, genericType );
	}

	public IInjectionBinding CrossContext()
	{
		_isCrossContext = true;
		if( resolver != null )
		{
			resolver( this );
		}
		return this;
	}

	public new IInjectionBinding Bind<T>()
	{
		return base.Bind<T>() as IInjectionBinding;
	}

	public new IInjectionBinding Bind( object key )
	{
		return base.Bind( key ) as IInjectionBinding;
	}

	public new IInjectionBinding To<T>()
	{
		return base.To<T>() as IInjectionBinding;
	}

	public new IInjectionBinding To( object o )
	{
		return base.To( o ) as IInjectionBinding;
	}

	public new IInjectionBinding ToName<T>()
	{
		return base.ToName<T>() as IInjectionBinding;
	}

	public new IInjectionBinding ToName( object o )
	{
		return base.ToName( o ) as IInjectionBinding;
	}

	public new IInjectionBinding Named<T>()
	{
		return base.Named<T>() as IInjectionBinding;
	}

	public new IInjectionBinding Named( object o )
	{
		return base.Named( o ) as IInjectionBinding;
	}
}
}
