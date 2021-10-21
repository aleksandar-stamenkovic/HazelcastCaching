import com.hazelcast.nio.serialization.DataSerializableFactory;
import com.hazelcast.nio.serialization.IdentifiedDataSerializable;

public class ProductSerializableFactory implements DataSerializableFactory {
	public static final int FACTORY_ID = 1000;

    @Override
   public IdentifiedDataSerializable create(int typeId) {
       if (typeId == 100) {
           return new Product();
       }
       return null;
   }
}
