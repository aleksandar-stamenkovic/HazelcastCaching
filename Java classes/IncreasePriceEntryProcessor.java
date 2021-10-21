

import com.hazelcast.map.EntryProcessor;
import com.hazelcast.nio.ObjectDataInput;
import com.hazelcast.nio.ObjectDataOutput;
import com.hazelcast.nio.serialization.IdentifiedDataSerializable;
import java.io.IOException;
import java.util.Map;

public class IncreasePriceEntryProcessor implements EntryProcessor<String, Product, Product>, IdentifiedDataSerializable {
	
	static final int CLASS_ID = 1;
    private float value;

   public IncreasePriceEntryProcessor() {
   }

    @Override
   public int getFactoryId() {
       return IncreasePriceFactory.FACTORY_ID;
   }

    @Override
   public int getClassId() {
       return CLASS_ID;
   }

    @Override
   public void writeData(ObjectDataOutput out) throws IOException {
       out.writeFloat(value);
   }

    @Override
   public void readData(ObjectDataInput in) throws IOException {
       value = in.readFloat();
   }

    @Override
    public Product process(Map.Entry<String, Product> entry) {
        Product product = entry.getValue();
        product.price *= 1.0 + value / 100.0;
        entry.setValue(product);
        return product;
    }
   
    











}