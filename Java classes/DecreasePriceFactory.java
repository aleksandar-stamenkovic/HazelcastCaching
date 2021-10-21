import com.hazelcast.nio.serialization.DataSerializableFactory;
import com.hazelcast.nio.serialization.IdentifiedDataSerializable;

public class DecreasePriceFactory implements DataSerializableFactory {
    public static final int FACTORY_ID = 6;

     @Override
    public IdentifiedDataSerializable create(int typeId) {
        if (typeId == DecreasePriceEntryProcessor.CLASS_ID) {
            return new DecreasePriceEntryProcessor();
        }
        return null;
    }
}