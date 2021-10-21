import com.hazelcast.nio.serialization.DataSerializableFactory;
import com.hazelcast.nio.serialization.IdentifiedDataSerializable;

public class IncreasePriceFactory implements DataSerializableFactory {
    public static final int FACTORY_ID = 5;

     @Override
    public IdentifiedDataSerializable create(int typeId) {
        if (typeId == IncreasePriceEntryProcessor.CLASS_ID) {
            return new IncreasePriceEntryProcessor();
        }
        return null;
    }
}