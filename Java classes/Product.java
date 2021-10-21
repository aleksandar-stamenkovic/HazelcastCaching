import java.io.IOException;

import com.hazelcast.nio.ObjectDataInput;
import com.hazelcast.nio.ObjectDataOutput;
import com.hazelcast.nio.serialization.IdentifiedDataSerializable;

public class Product implements IdentifiedDataSerializable {
	
	static final int CLASS_ID = 100;
	
	public String name;
	public float price;
	public String description;
	
	public Product()
	{
	}
	
	public Product (String name, float price, String description)
	{
		this.name=name;
		this.price=price;
		this.description=description;
	}

	@Override
	public void readData(ObjectDataInput in) throws IOException {
		name = in.readString();
		price = in.readFloat();
		description = in.readString();
		
	}

	@Override
	public void writeData(ObjectDataOutput out) throws IOException {
		out.writeString(name);
		out.writeFloat(price);
		out.writeString(description);
		
	}

	@Override
	public int getClassId() {
		return CLASS_ID;
	}

	@Override
	public int getFactoryId() {
		return ProductSerializableFactory.FACTORY_ID;
	}
}
